<script lang="ts">
	import { createTeamsList, type EntitiesTeam } from "$lib/api";
	import { Button, Card, Empty } from "@kayord/ui";
	import PlusIcon from "@lucide/svelte/icons/plus";
	import Actions from "./Actions.svelte";
	import AddTeam from "./AddTeam.svelte";
	import { BirdIcon } from "@lucide/svelte";

	const query = createTeamsList();

	let open = $state(false);
</script>

{#snippet teamSnippet(team: EntitiesTeam)}
	<Card.Root>
		<Card.Header class="flex flex-row items-center">
			<div class="flex w-full flex-col items-start">
				<Card.Title>{team.name}</Card.Title>
			</div>
			<Actions {team} refetch={query.refetch} />
		</Card.Header>
	</Card.Root>
{/snippet}

{#snippet addButton()}
	<div class="flex justify-end">
		<Button class="my-2" onclick={() => (open = true)}>
			<PlusIcon />Add new Team
		</Button>
	</div>
{/snippet}

<div class="m-2">
	{@render addButton()}

	{#if query.data?.length == 0}
		<Empty.Root>
			<Empty.Header>
				<Empty.Media variant="icon">
					<BirdIcon />
				</Empty.Media>
				<Empty.Title>No Teams</Empty.Title>
				<Empty.Description>You haven't created any teams yet.</Empty.Description>
			</Empty.Header>
			<Empty.Content>
				{@render addButton()}
			</Empty.Content>
		</Empty.Root>
	{/if}

	<div class="flex flex-col gap-2">
		{#each query.data ?? [] as team (team.id)}
			{@render teamSnippet(team)}
		{/each}
	</div>

	{#if open}
		<AddTeam bind:open refetch={query.refetch} />
	{/if}
</div>
