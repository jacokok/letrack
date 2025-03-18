<script lang="ts">
	import { createRaceDelete, type EntitiesPlayer, type EntitiesRace } from "$lib/api";
	import { AlertDialog, Button, DropdownMenu, toast } from "@kayord/ui";
	import PencilIcon from "@lucide/svelte/icons/pencil";
	import ViewIcon from "@lucide/svelte/icons/view";
	import Trash2Icon from "@lucide/svelte/icons/trash-2";
	import EllipsisVerticalIcon from "@lucide/svelte/icons/ellipsis-vertical";
	import { getError } from "$lib/types";
	import AddRace from "./AddRace.svelte";
	import { goto } from "$app/navigation";
	import { ChartNoAxesCombinedIcon } from "@lucide/svelte";

	interface Props {
		refetch: () => void;
		race: EntitiesRace;
	}

	let { race, refetch }: Props = $props();

	let deleteOpen = $state(false);
	let editOpen = $state(false);

	const deleteMutation = createRaceDelete();
	const deleteMenu = async () => {
		deleteOpen = false;
		try {
			await $deleteMutation.mutateAsync({ id: race.id });
			refetch();
			toast.message("Race Deleted");
		} catch (error) {
			toast.error(getError(error).message);
		}
	};
</script>

<DropdownMenu.Root>
	<DropdownMenu.Trigger>
		<Button size="icon" variant="default">
			<EllipsisVerticalIcon class="size-5" />
		</Button>
	</DropdownMenu.Trigger>
	<DropdownMenu.Content>
		<DropdownMenu.Item onclick={() => goto(`/race/${race.id}`)}>
			<ViewIcon /> View
		</DropdownMenu.Item>
		<DropdownMenu.Item onclick={() => goto(`/race/${race.id}/stats`)}>
			<ChartNoAxesCombinedIcon /> Stats
		</DropdownMenu.Item>
		<DropdownMenu.Item onclick={() => (editOpen = true)}><PencilIcon /> Edit</DropdownMenu.Item>
		<DropdownMenu.Item onclick={() => (deleteOpen = true)}><Trash2Icon /> Delete</DropdownMenu.Item>
	</DropdownMenu.Content>
</DropdownMenu.Root>

<AddRace {refetch} bind:open={editOpen} {race} />

<AlertDialog.Root bind:open={deleteOpen}>
	<AlertDialog.Content>
		<AlertDialog.Header>
			<AlertDialog.Title>Delete Race?</AlertDialog.Title>
			<AlertDialog.Description>
				This will delete the race and all lap information related to it.
			</AlertDialog.Description>
		</AlertDialog.Header>
		<AlertDialog.Footer>
			<AlertDialog.Cancel>Cancel</AlertDialog.Cancel>
			<AlertDialog.Action class="bg-destructive text-destructive-foreground" onclick={deleteMenu}>
				Delete
			</AlertDialog.Action>
		</AlertDialog.Footer>
	</AlertDialog.Content>
</AlertDialog.Root>
