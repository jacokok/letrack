<script lang="ts">
	import type { RaceSummaryTrack } from "$lib/api";
	import { timeSpanToParts } from "$lib/util";
	import { Badge, Card } from "@kayord/ui";
	import FastestIcon from "lucide-svelte/icons/zap";

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
		{track.player.name}<span class="text-muted-foreground text-sm"
			>{track.player.nickName ? ` (${track.player.nickName})` : ""}</span
		>
	</h1>

	<Badge class="bg-muted text-muted-foreground">
		<FastestIcon class="mr-1 size-3" />
		{timeSpanToParts(track.fastestLap?.lapTime).value}
	</Badge>
</Card.Root>
