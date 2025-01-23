<script lang="ts">
	import type { EntitiesRace } from "$lib/api";
	import { Badge, Card } from "@kayord/ui";
	import Actions from "./Actions.svelte";

	interface Props {
		race: EntitiesRace;
		refetch: () => void;
	}

	let { race, refetch }: Props = $props();
</script>

<Card.Root>
	<Card.Header class="flex flex-row items-center">
		<div class="flex w-full flex-col items-start gap-1">
			<Card.Title>{race.name}</Card.Title>
			<Card.Description>{new Date(race.createdDateTime).toLocaleString()}</Card.Description>
			{#if race.isActive}
				<Badge class="animate-pulse">Active</Badge>
			{/if}
		</div>
		<Actions {race} {refetch} />
	</Card.Header>
	<Card.Content>
		<div class="flex items-center gap-4">
			{#each race.raceTracks as track}
				<Card.Root class="p-2">
					<div class="flex items-center gap-2">
						<Badge>Track {track.trackId}</Badge>
						{track.player.name} ({track.player.nickName})
					</div>
				</Card.Root>
			{/each}
		</div>
	</Card.Content>
</Card.Root>
