<script lang="ts">
	import type { RaceSummaryTrack } from "$lib/api";
	import PlayerAvatar from "$lib/components/PlayerAvatar.svelte";
	import { timeSpanToParts } from "$lib/util";
	import { Badge, Card } from "@kayord/ui";
	import { CarIcon, LandPlotIcon } from "@lucide/svelte";
	import FastestIcon from "@lucide/svelte/icons/zap";

	interface Props {
		track: RaceSummaryTrack;
	}
	let { track }: Props = $props();
</script>

<Card.Root class="flex flex-row items-center justify-between gap-1 p-1">
	<div class="flex flex-col gap-1">
		<Badge variant="outline">Track {track.trackId}</Badge>
		<Badge class="bg-muted text-muted-foreground"><CarIcon /> {track.totalLaps}</Badge>
	</div>

	<h1 class="flex items-center gap-1">
		<PlayerAvatar name={track.player.name} isSmall />
		<div class="flex shrink flex-col">
			<div class="leading-4">
				{track.player.nickName || track.player.name} asdf asd fa fadsf sfd
			</div>
			<div class="text-muted-foreground text-xs leading-3">
				{track.player.teamName}
			</div>
		</div>
	</h1>

	<Badge class="bg-muted text-muted-foreground">
		<FastestIcon class="size-3" />
		{timeSpanToParts(track.fastestLap?.lapTime).value}
	</Badge>
</Card.Root>
