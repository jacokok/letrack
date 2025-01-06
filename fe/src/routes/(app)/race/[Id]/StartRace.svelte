<script lang="ts">
	import { Button, Checkbox, Dialog, Form, Input } from "@kayord/ui";
	import { defaults, superForm } from "sveltekit-superforms";
	import { zod } from "sveltekit-superforms/adapters";
	import { z } from "zod";

	interface Props {
		cb: (data: { duration: number; laps: number; showCountdown: boolean }) => void;
		hideOptions?: boolean;
		open?: boolean;
	}

	let { cb, open = $bindable(false), hideOptions = false }: Props = $props();

	const schema = z.object({
		duration: z.coerce.number(),
		laps: z.coerce.number(),
		showCountdown: z.boolean()
	});

	type FormSchema = z.infer<typeof schema>;

	const form = superForm(defaults({ duration: 0, laps: 0, showCountdown: true }, zod(schema)), {
		SPA: true,
		validators: zod(schema),
		onUpdate({ form }) {
			if (form.valid) {
				startRace(form.data);
			}
		}
	});

	const startRace = async (data: FormSchema) => {
		cb(data);
	};

	const { form: formData, enhance } = form;
</script>

<Dialog.Root bind:open>
	<Dialog.Content class="max-h-[98%] overflow-scroll">
		<form method="POST" use:enhance>
			<Dialog.Header>
				<Dialog.Title>Start Race</Dialog.Title>
				{#if !hideOptions}
					<Dialog.Description>Start Race Options</Dialog.Description>
				{/if}
			</Dialog.Header>
			<div class="flex flex-col gap-4 p-4">
				<Form.Field {form} name="showCountdown">
					<Form.Control>
						{#snippet children({ props })}
							<div class="flex gap-2">
								<Form.Label>Show Countdown</Form.Label>
								<Checkbox {...props} bind:checked={$formData.showCountdown} />
							</div>
						{/snippet}
					</Form.Control>
					<Form.FieldErrors />
				</Form.Field>
				{#if !hideOptions}
					<Form.Field {form} name="duration">
						<Form.Control>
							{#snippet children({ props })}
								<Form.Label>Duration</Form.Label>
								<Input {...props} bind:value={$formData.duration} />
							{/snippet}
						</Form.Control>
						<Form.Description>End Race After Duration in minutes (0 for unlimited)</Form.Description
						>
						<Form.FieldErrors />
					</Form.Field>
					<Form.Field {form} name="laps">
						<Form.Control>
							{#snippet children({ props })}
								<Form.Label>Laps</Form.Label>
								<Input {...props} bind:value={$formData.laps} />
							{/snippet}
						</Form.Control>
						<Form.Description>End Race After Number of Laps. (0 for unlimited)</Form.Description>
						<Form.FieldErrors />
					</Form.Field>
				{/if}
			</div>
			<Dialog.Footer class="gap-2">
				<Button type="submit">Start Race</Button>
				<Button variant="outline" onclick={() => (open = false)}>Cancel</Button>
			</Dialog.Footer>
		</form>
	</Dialog.Content>
</Dialog.Root>
