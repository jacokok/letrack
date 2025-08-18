<script lang="ts">
	import {
		createRaceUpdate,
		createRaceInsert,
		type EntitiesRace,
		createPlayersList
	} from "$lib/api";
	import { getError } from "$lib/types";
	import { Button, Dialog, Form, Input, Select, Switch, toast } from "@kayord/ui";
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

	const schema = z.object({
		name: z.string().min(1, { message: "Name is Required" }),
		isFirstTracks: z.boolean(),
		players: z
			.array(z.coerce.number().min(1, { message: "Please select player" }))
			.min(2, { message: "Please add one player" })
	});
	type FormSchema = z.infer<typeof schema>;

	const isFirstTracks = $derived((race?.raceTracks.filter((c) => c.trackId > 2).length ?? 0) == 0);

	const defaultValues = $derived({
		name: race?.name ?? "",
		players: race?.raceTracks.map((t) => t.playerId) ?? [0, 0],
		isFirstTracks
	});

	const editMutation = createRaceUpdate();
	const createMutation = createRaceInsert();

	const playersQuery = createPlayersList();
	const players = $derived($playersQuery?.data ?? []);

	const updateExtra = async (data: FormSchema) => {
		try {
			open = false;
			if (isEdit) {
				await $editMutation.mutateAsync({
					data: {
						name: data.name,
						players: data.players,
						id: race?.id ?? 0,
						isFirstTracks: data.isFirstTracks
					}
				});
				toast.info("Edited race");
			} else {
				await $createMutation.mutateAsync({
					data: { name: data.name, players: data.players, isFirstTracks: data.isFirstTracks }
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

	const { form: formData, enhance, reset, restore } = form;

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

				<Form.Field {form} name="isFirstTracks">
					<Form.Control>
						{#snippet children({ props })}
							<div class="flex items-center justify-start gap-2">
								<Switch {...props} bind:checked={$formData.isFirstTracks} />
								<Form.Label>First Tracks</Form.Label>
							</div>
						{/snippet}
					</Form.Control>
					<Form.FieldErrors />
				</Form.Field>

				<Form.Field {form} name="players[0]">
					<Form.Control>
						{#snippet children({ props })}
							<Form.Label>Track {$formData.isFirstTracks ? 1 : 3}</Form.Label>
							<Select.Root
								type="single"
								bind:value={
									() => $formData.players[0].toString(), (v) => ($formData.players[0] = Number(v))
								}
								name={props.name}
							>
								<Select.Trigger {...props}>
									{players.find((i) => i.id === $formData.players[0])?.name ?? "Select a Player"}
								</Select.Trigger>
								<Select.Content>
									{#each players as player}
										<Select.Item value={player.id.toString()} label={player.name}>
											{player.name}
										</Select.Item>
									{/each}
								</Select.Content>
							</Select.Root>
						{/snippet}
					</Form.Control>
					<Form.FieldErrors />
				</Form.Field>

				<Form.Field {form} name="players[1]">
					<Form.Control>
						{#snippet children({ props })}
							<Form.Label>Track {$formData.isFirstTracks ? 2 : 4}</Form.Label>
							<Select.Root
								type="single"
								bind:value={
									() => $formData.players[1].toString(), (v) => ($formData.players[1] = Number(v))
								}
								name={props.name}
							>
								<Select.Trigger {...props}>
									{players.find((i) => i.id === $formData.players[1])?.name ?? "Select a Player"}
								</Select.Trigger>
								<Select.Content>
									{#each players as player}
										<Select.Item value={player.id.toString()} label={player.name}>
											{player.name}
										</Select.Item>
									{/each}
								</Select.Content>
							</Select.Root>
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
