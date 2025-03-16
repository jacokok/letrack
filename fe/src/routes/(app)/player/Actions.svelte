<script lang="ts">
	import { createPlayersDelete, type EntitiesPlayer } from "$lib/api";
	import { AlertDialog, Button, DropdownMenu, toast } from "@kayord/ui";
	import PencilIcon from "@lucide/svelte/icons/pencil";
	import Trash2Icon from "@lucide/svelte/icons/trash-2";
	import EllipsisVerticalIcon from "@lucide/svelte/icons/ellipsis-vertical";
	import { getError } from "$lib/types";
	import AddPlayer from "./AddPlayer.svelte";

	interface Props {
		refetch: () => void;
		player: EntitiesPlayer;
	}

	let { player, refetch }: Props = $props();

	let deleteOpen = $state(false);
	let editOpen = $state(false);

	const deleteMutation = createPlayersDelete();
	const deleteMenu = async () => {
		deleteOpen = false;
		try {
			await $deleteMutation.mutateAsync({ id: player.id });
			refetch();
			toast.message("Player Deleted");
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
		<DropdownMenu.Item onclick={() => (editOpen = true)}><PencilIcon /> Edit</DropdownMenu.Item>
		<DropdownMenu.Item onclick={() => (deleteOpen = true)}><Trash2Icon /> Delete</DropdownMenu.Item>
	</DropdownMenu.Content>
</DropdownMenu.Root>

<AddPlayer {refetch} bind:open={editOpen} {player} />

<AlertDialog.Root bind:open={deleteOpen}>
	<AlertDialog.Content>
		<AlertDialog.Header>
			<AlertDialog.Title>Delete Player?</AlertDialog.Title>
			<AlertDialog.Description>
				This will delete the player and all player related data.
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
