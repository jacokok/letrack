<script lang="ts">
	import { createTeamsList, type EntitiesTeam } from "$lib/api";
	import { Button, Card } from "@kayord/ui";
	import PlusIcon from "@lucide/svelte/icons/plus";
	import Actions from "./Actions.svelte";
	import AddTeam from "./AddTeam.svelte";

	const query = createTeamsList();

	let open = $state(false);
</script>

{#snippet teamSnippet(team: EntitiesTeam)}
	<Card.Root>
		<Card.Header class="flex flex-row items-center">
			<div class="flex w-full flex-col items-start">
				<Card.Title>{team.name}</Card.Title>
			</div>
			<Actions {team} refetch={$query.refetch} />
		</Card.Header>
	</Card.Root>
{/snippet}

<div class="m-2">
	<div class="flex justify-end">
		<Button class="my-2" onclick={() => (open = true)}>
			<PlusIcon />Add new Team
		</Button>
	</div>

	{#if $query.data?.length == 0}
		<Card.Root class="p-2">No teams</Card.Root>
	{/if}

	<div class="flex flex-col gap-2">
		{#each $query.data ?? [] as team}
			{@render teamSnippet(team)}
		{/each}
	</div>

	<AddTeam bind:open refetch={$query.refetch} />
</div>
