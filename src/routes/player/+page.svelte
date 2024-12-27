<script lang="ts">
	import { createPlayersList, type EntitiesPlayer } from "$lib/api";
	import { Button, Card } from "@kayord/ui";
	import PlusIcon from "lucide-svelte/icons/plus";
	import AddPlayer from "./AddPlayer.svelte";
	import Actions from "./Actions.svelte";

	const query = createPlayersList();

	let open = $state(false);
</script>

{#snippet playerSnippet(player: EntitiesPlayer)}
	<Card.Root class="m-2">
		<Card.Header class="mb-6 flex flex-row items-center">
			<div class="flex w-full flex-col items-start">
				<Card.Title>{player.name}</Card.Title>
				<Card.Description>{player.nickName}</Card.Description>
			</div>
			<Actions {player} refetch={$query.refetch} />
		</Card.Header>
	</Card.Root>
{/snippet}

<div class="m2">
	{#if $query.data?.length == 0}
		<Card.Root class="m-2 p-2">No players</Card.Root>
	{/if}
	{#each $query.data ?? [] as player}
		{@render playerSnippet(player)}
	{/each}
	<Button class="w-full" onclick={() => (open = true)}>
		<PlusIcon />Add new Player
	</Button>

	<AddPlayer bind:open refetch={$query.refetch} />
</div>
