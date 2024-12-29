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
	<Badge class="bg-muted text-muted-foreground">Laps: {track.totalLaps}</Badge>
	<h1 class="flex items-center gap-1">
		{track.player.name}<span class="text-sm text-muted-foreground"
			>{track.player.nickName ? ` (${track.player.nickName})` : ""}</span
		>
	</h1>
	<Badge class="bg-muted text-muted-foreground">
		<FastestIcon class="mr-1 size-3" />
		{timeSpanToParts(track.fastestLap?.lapTime).value}
	</Badge>
</Card.Root>
