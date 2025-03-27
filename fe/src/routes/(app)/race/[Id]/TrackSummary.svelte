<script lang="ts">
	import type { RaceSummaryTrack } from "$lib/api";
	import PlayerAvatar from "$lib/components/PlayerAvatar.svelte";
	import { timeSpanToParts } from "$lib/util";
	import { Badge, Card } from "@kayord/ui";
	import FastestIcon from "@lucide/svelte/icons/zap";

	interface Props {
		track: RaceSummaryTrack;
	}
	let { track }: Props = $props();
</script>

<Card.Root class="flex items-center justify-between p-2">
	<div class="flex flex-col gap-1">
		<Badge variant="outline">Track {track.trackId}</Badge>
		<Badge class="bg-muted text-muted-foreground">Laps: {track.totalLaps}</Badge>
	</div>

	<h1 class="flex items-center gap-1">
		<PlayerAvatar name={track.player.name} isSmall />
		<div class="flex flex-col">
			<div class="leading-4">
				{track.player.name}
			</div>
			<div class="text-muted-foreground text-xs leading-3">
				{track.player.nickName ? ` ${track.player.nickName}` : ""}
			</div>
		</div>
	</h1>

	<Badge class="bg-muted text-muted-foreground">
		<FastestIcon class="mr-1 size-3" />
		{timeSpanToParts(track.fastestLap?.lapTime).value}
	</Badge>
</Card.Root>
