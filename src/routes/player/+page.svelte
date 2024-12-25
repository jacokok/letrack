<script lang="ts">
	import { createPlayersList, type EntitiesPlayer } from "$lib/api";
	import { Button, Card } from "@kayord/ui";
	import PlusIcon from "lucide-svelte/icons/plus";

	const query = createPlayersList();
</script>

{#snippet playerSnippet(player: EntitiesPlayer)}
	<Card.Root class="m-2 p-2">
		{player.name}
	</Card.Root>
{/snippet}

<div class="m2">
	{#if $query.data?.length == 0}
		<Card.Root class="m-2 p-2">No players</Card.Root>
	{/if}
	{#each $query.data ?? [] as player}
		{@render playerSnippet(player)}
	{/each}
	<Button class="w-full"><PlusIcon />Add new Player</Button>
</div>
