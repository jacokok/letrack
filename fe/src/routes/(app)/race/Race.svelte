<script lang="ts">
	import type { EntitiesRace } from "$lib/api";
	import { Badge, Card } from "@kayord/ui";
	import Actions from "./Actions.svelte";
	import { cn } from "@kayord/ui/utils";

	interface Props {
		race: EntitiesRace;
		refetch: () => void;
	}

	let { race, refetch }: Props = $props();

	const getVariant = (track: number) => {
		if (track <= 1) {
			return "bg-primary text-primary-foreground";
		} else if (track == 2) {
			return "bg-destructive text-destructive-foreground hover:bg-destructive/80";
		} else if (track == 3) {
			return "bg-accent text-accent-foreground hover:bg-accent/80";
		} else return "bg-secondary text-secondary-foreground hover:bg-secondary/80";
	};

	const isFinished = $derived(
		race.isActive == false && (race.endDateTime != null || race.endLapCount != null)
	);
</script>

<Card.Root
	class={`border-2 ${race.isActive ? "border-primary" : "border-muted"} ${isFinished ? "grayscale" : ""}`}
>
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
						<Badge class={cn(getVariant(track.trackId))}>Track {track.trackId}</Badge>
						{track.player.name} ({track.player.nickName})
					</div>
				</Card.Root>
			{/each}
		</div>
	</Card.Content>
</Card.Root>
