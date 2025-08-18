<script lang="ts">
	import { createTeamsInsert, createTeamsUpdate, type EntitiesTeam } from "$lib/api";
	import { getError } from "$lib/types";
	import { Button, Dialog, Form, Input, toast } from "@kayord/ui";
	import { defaults, superForm } from "sveltekit-superforms";
	import { zod4 } from "sveltekit-superforms/adapters";
	import { z } from "zod";

	interface Props {
		refetch: () => void;
		open?: boolean;
		team?: EntitiesTeam;
	}

	let { refetch, open = $bindable(false), team }: Props = $props();

	const isEdit = $derived(team != null);

	const schema = z.object({
		name: z.string().min(1, { message: "Name is Required" })
	});
	type FormSchema = z.infer<typeof schema>;

	const defaultValues = $derived({
		name: team?.name ?? ""
	});

	const editMutation = createTeamsUpdate();
	const createMutation = createTeamsInsert();

	const updateExtra = async (data: FormSchema) => {
		try {
			open = false;
			if (isEdit) {
				await $editMutation.mutateAsync({
					data: {
						id: team?.id ?? 0,
						name: data.name
					}
				});
				toast.info("Edited team");
			} else {
				await $createMutation.mutateAsync({ data: { name: data.name } });
				toast.info("Added team");
			}
			refetch();
		} catch (err) {
			toast.error(getError(err).message);
		}
	};

	// svelte-ignore state_referenced_locally
	const form = superForm(defaults(defaultValues, zod4(schema)), {
		SPA: true,
		validators: zod4(schema),
		onUpdate({ form }) {
			if (form.valid) {
				updateExtra(form.data);
			}
		}
	});

	const { form: formData, enhance, reset } = form;

	$effect(() => {
		if (open == true) {
			reset({ data: defaultValues });
		}
	});
</script>

<Dialog.Root bind:open>
	<Dialog.Content class="max-h-[98%] overflow-auto">
		<form method="POST" use:enhance>
			<Dialog.Header>
				<Dialog.Title>{isEdit ? "Edit" : "Add"} Team</Dialog.Title>
				<Dialog.Description>Complete form to {isEdit ? "Edit" : "Add"} Team</Dialog.Description>
			</Dialog.Header>
			<div class="flex flex-col gap-4 p-4">
				<Form.Field {form} name="name">
					<Form.Control>
						{#snippet children({ props })}
							<Form.Label>Name</Form.Label>
							<Input {...props} bind:value={$formData.name} />
						{/snippet}
					</Form.Control>
					<Form.FieldErrors />
				</Form.Field>
			</div>
			<Dialog.Footer class="gap-2">
				<Button type="submit">Submit</Button>
				<Button variant="outline" onclick={() => (open = false)}>Cancel</Button>
			</Dialog.Footer>
		</form>
	</Dialog.Content>
</Dialog.Root>
