<script lang="ts">
	import {
		createPlayersInsert,
		createPlayersUpdate,
		type EntitiesPlayer,
		createTeamsList
	} from "$lib/api";
	import { getError } from "$lib/types";
	import { Button, Dialog, Input, Select } from "@kayord/ui";
	import { toast } from "@kayord/ui/sonner";
	import { Form } from "@kayord/ui/form";
	import { defaults, superForm } from "sveltekit-superforms";
	import { zod4 } from "sveltekit-superforms/adapters";
	import { z } from "zod";

	interface Props {
		refetch: () => void;
		open?: boolean;
		player?: EntitiesPlayer;
	}

	let { refetch, open = $bindable(false), player }: Props = $props();

	const isEdit = $derived(player != null);

	const schema = z.object({
		name: z.string().min(1, { message: "Name is Required" }),
		nickName: z.string(),
		teamId: z.number().min(1, { message: "Team is required" }),
		order: z.number()
	});
	type FormSchema = z.infer<typeof schema>;

	const defaultValues = $derived({
		name: player?.name ?? "",
		nickName: player?.nickName ?? "",
		teamId: player?.teamId ?? 0,
		order: player?.order ?? 0
	});

	const teamsQuery = createTeamsList();

	const editMutation = createPlayersUpdate();
	const createMutation = createPlayersInsert();

	const updateExtra = async (data: FormSchema) => {
		try {
			open = false;
			if (isEdit) {
				await editMutation.mutateAsync({
					data: {
						id: player?.id ?? 0,
						name: data.name,
						nickName: data.nickName,
						teamId: data.teamId,
						order: data.order
					}
				});
				toast.info("Edited player");
			} else {
				await createMutation.mutateAsync({
					data: { name: data.name, nickName: data.nickName, teamId: data.teamId, order: data.order }
				});
				toast.info("Added player");
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

	const selectedTeam = $derived(teamsQuery.data?.find((x) => x.id == $formData.teamId));
</script>

<Dialog.Root bind:open>
	<Dialog.Content class="max-h-[98%] overflow-auto">
		<form method="POST" use:enhance>
			<Dialog.Header>
				<Dialog.Title>{isEdit ? "Edit" : "Add"} Player</Dialog.Title>
				<Dialog.Description>Complete form to {isEdit ? "Edit" : "Add"} Player</Dialog.Description>
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
				<Form.Field {form} name="nickName">
					<Form.Control>
						{#snippet children({ props })}
							<Form.Label>Nick Name</Form.Label>
							<Input {...props} bind:value={$formData.nickName} />
						{/snippet}
					</Form.Control>
					<Form.FieldErrors />
				</Form.Field>
				<Form.Field {form} name="teamId">
					<Form.Control>
						{#snippet children({ props })}
							<Form.Label>Team</Form.Label>
							<Select.Root
								type="single"
								bind:value={
									() => $formData.teamId.toString(), (v) => ($formData.teamId = Number(v))
								}
								name={props.name}
							>
								<Select.Trigger {...props}>
									{selectedTeam ? selectedTeam.name : "Select a Team"}
								</Select.Trigger>
								<Select.Content>
									{#each teamsQuery.data ?? [] as team (team.id)}
										<Select.Item value={team.id.toString()} label={team.name} />
									{/each}
								</Select.Content>
							</Select.Root>
						{/snippet}
					</Form.Control>
					<Form.FieldErrors />
				</Form.Field>
				<Form.Field {form} name="order">
					<Form.Control>
						{#snippet children({ props })}
							<Form.Label>Order</Form.Label>
							<Input {...props} bind:value={$formData.order} type="number" />
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
