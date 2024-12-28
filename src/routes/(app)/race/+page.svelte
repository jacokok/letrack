<script lang="ts">
	import { goto } from "$app/navigation";
	import { Button, Card } from "@kayord/ui";
	import PlusIcon from "lucide-svelte/icons/plus";
	import HistoryIcon from "lucide-svelte/icons/history";
	import { createRaceList } from "$lib/api";
	import Race from "./Race.svelte";
	import AddRace from "./AddRace.svelte";

	const query = createRaceList({ isActive: true });

	let open = $state(false);
</script>

<div class="m-2">
	<div class="flex items-center justify-end gap-2">
		<Button variant="secondary" onclick={() => goto("/race/history")}>
			<HistoryIcon />History
		</Button>
		<Button class="my-2" onclick={() => (open = true)}>
			<PlusIcon />Add Race
		</Button>
	</div>

	{#if $query.data?.length == 0}
		<Card.Root class="p-2">No Races</Card.Root>
	{/if}

	<div class="flex flex-col gap-2">
		{#each $query.data ?? [] as race}
			<Race {race} refetch={$query.refetch} />
		{/each}
	</div>

	<AddRace bind:open refetch={$query.refetch} />
</div>
