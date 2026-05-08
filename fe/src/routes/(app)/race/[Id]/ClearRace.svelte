<script lang="ts">
	import { AlertDialog, Button } from "@kayord/ui";
	import { TrashIcon } from "@lucide/svelte";
	import { createRaceClear } from "$lib/api";
	import { toast } from "@kayord/ui/sonner";
	import { getError } from "$lib/types";

	interface Props {
		raceId: number;
		refetch: () => void;
	}
	let { raceId, refetch }: Props = $props();

	let deleteOpen = $state(false);

	const clearMutation = createRaceClear();
	const clearRace = async () => {
		deleteOpen = false;
		try {
			await clearMutation.mutateAsync({ data: { id: raceId } });
			refetch();
			toast.message("Race Cleared");
		} catch (error) {
			toast.error(getError(error).message);
		}
	};
</script>

<Button variant="destructive" onclick={() => (deleteOpen = true)}>
	<TrashIcon />
</Button>

<AlertDialog.Root bind:open={deleteOpen}>
	<AlertDialog.Content>
		<AlertDialog.Header>
			<AlertDialog.Title>Clear Race?</AlertDialog.Title>
			<AlertDialog.Description>
				This will clear all laps for this race. Be careful?
			</AlertDialog.Description>
		</AlertDialog.Header>
		<AlertDialog.Footer>
			<AlertDialog.Cancel>Cancel</AlertDialog.Cancel>
			<AlertDialog.Action class="bg-destructive text-destructive-foreground" onclick={clearRace}>
				Clear
			</AlertDialog.Action>
		</AlertDialog.Footer>
	</AlertDialog.Content>
</AlertDialog.Root>
