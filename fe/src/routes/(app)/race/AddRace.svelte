<script lang="ts">
	import {
		createRaceUpdate,
		createRaceInsert,
		type EntitiesRace,
		createPlayersList
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
		race?: EntitiesRace;
	}

	let { refetch, open = $bindable(false), race }: Props = $props();

	const isEdit = $derived(race != null);

	function hasDuplicates<T>(arr: T[]): boolean {
		return new Set(arr).size !== arr.length;
	}

	const schema = z
		.object({
			name: z.string().min(1, { message: "Name is Required" }),
			players: z
				.array(z.coerce.number().min(1, { message: "Please select player" }))
				.min(4, { message: "Please add one player" })
		})
		.superRefine((items, ctx) => {
			const teams = items.players.map((x) => players.find((i) => i.id === x)?.teamId ?? 0);
			if (hasDuplicates(teams))
				ctx.addIssue({
					code: z.ZodIssueCode.custom,
					message: "Invalid teams",
					path: ["name"]
				});
		});
	type FormSchema = z.infer<typeof schema>;

	const defaultValues = $derived({
		name: race?.name ?? "",
		players: race?.raceTracks.map((t) => t.playerId) ?? [0, 0, 0, 0]
	});

	const editMutation = createRaceUpdate();
	const createMutation = createRaceInsert();

	const playersQuery = createPlayersList();
	const players = $derived(playersQuery?.data ?? []);

	const updateExtra = async (data: FormSchema) => {
		try {
			open = false;
			if (isEdit) {
				await editMutation.mutateAsync({
					data: {
						name: data.name,
						players: data.players,
						id: race?.id ?? 0
					}
				});
				toast.info("Edited race");
			} else {
				await createMutation.mutateAsync({
					data: { name: data.name, players: data.players }
				});
				toast.info("Added race");
			}
			refetch();
		} catch (err) {
			toast.error(getError(err).message);
		}
	};

	// svelte-ignore state_referenced_locally
	const form = superForm(defaults(defaultValues, zod4(schema)), {
		SPA: true,
		id: `race-form-${isEdit ? "edit" : "create"}`,
		dataType: "json",
		validators: zod4(schema),
		onUpdate({ form }) {
			if (form.valid) {
				updateExtra(form.data);
			}
		}
	});

	const { form: formData, enhance, reset } = form;

	$effect(() => {
		if (open == true && isEdit) {
			reset({ data: defaultValues, id: `race-form-${isEdit ? "edit" : "create"}` });
		}
	});
</script>

<Dialog.Root bind:open>
	<Dialog.Content class="max-h-[98%] overflow-auto">
		<form method="POST" use:enhance>
			<Dialog.Header>
				<Dialog.Title>{isEdit ? "Edit" : "Add"} Race</Dialog.Title>
				<Dialog.Description>Complete form to {isEdit ? "Edit" : "Add"} Race</Dialog.Description>
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

				{#each [0, 1, 2, 3] as trackIndex (trackIndex)}
					<Form.Field {form} name={`players[${trackIndex}]`}>
						<Form.Control>
							{#snippet children({ props })}
								<Form.Label>Track {trackIndex + 1}</Form.Label>
								<Select.Root
									type="single"
									bind:value={
										() => $formData.players[trackIndex].toString(),
										(v) => ($formData.players[trackIndex] = Number(v))
									}
									name={props.name}
								>
									<Select.Trigger {...props} class="w-full">
										{players.find((i) => i.id === $formData.players[trackIndex])?.name ??
											"Select a Player"}
									</Select.Trigger>
									<Select.Content class="max-h-60 overflow-y-auto">
										{#each players as player (player.id)}
											<Select.Item value={player.id.toString()} label={player.name}>
												{player.name}
											</Select.Item>
										{/each}
									</Select.Content>
								</Select.Root>
							{/snippet}
						</Form.Control>

						<Form.Legend class="text-muted-foreground">
							{players.find((i) => i.id === $formData.players[trackIndex])?.team?.name}
						</Form.Legend>
						<Form.FieldErrors />
					</Form.Field>
				{/each}
			</div>
			<Dialog.Footer class="gap-2">
				<Button type="submit">Submit</Button>
				<Button variant="outline" onclick={() => (open = false)}>Cancel</Button>
			</Dialog.Footer>
		</form>
	</Dialog.Content>
</Dialog.Root>
