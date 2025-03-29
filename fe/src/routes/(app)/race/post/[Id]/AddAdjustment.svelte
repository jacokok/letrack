<script lang="ts">
	import { createLapsAdjustment } from "$lib/api";
	import { getError } from "$lib/types";
	import { Button, Dialog, Input, Label, toast } from "@kayord/ui";

	interface Props {
		refetch: () => void;
		open: boolean;
		trackId: number;
		raceId: number;
	}

	let { refetch, open = $bindable(false), trackId, raceId }: Props = $props();

	let amount = $state(1);

	const createMutation = createLapsAdjustment();

	const onSubmit = async () => {
		try {
			open = false;
			await $createMutation.mutateAsync({
				data: { amount, raceId, trackId }
			});
			toast.info("Added adjustment");
			refetch();
		} catch (err) {
			toast.error(getError(err).message);
		}
	};
</script>

<Dialog.Root bind:open>
	<Dialog.Content class="max-h-[98%] overflow-scroll">
		<Dialog.Header>
			<Dialog.Title>Adjustment</Dialog.Title>
			<Dialog.Description>You want to manually add laps for some reason</Dialog.Description>
		</Dialog.Header>
		<div class="flex flex-col gap-4 p-4">
			<Label>Amount of Laps to add</Label>
			<Input bind:value={amount} type="number" />
		</div>
		<Dialog.Footer class="gap-2">
			<Button onclick={onSubmit}>Submit</Button>
			<Button variant="outline" onclick={() => (open = false)}>Cancel</Button>
		</Dialog.Footer>
	</Dialog.Content>
</Dialog.Root>
