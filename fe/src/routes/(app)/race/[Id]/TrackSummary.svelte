<script lang="ts">
	import type { RaceSummaryTrack } from "$lib/api";
	import PlayerAvatar from "$lib/components/PlayerAvatar.svelte";
	import { timeSpanToParts } from "$lib/util";
	import { Badge, Card } from "@kayord/ui";
	import { CarIcon } from "@lucide/svelte";
	import FastestIcon from "@lucide/svelte/icons/zap";

	interface Props {
		track: RaceSummaryTrack;
	}
	let { track }: Props = $props();
</script>

<div class="bg-background/50 flex min-w-0 flex-row items-center gap-1 p-1">
	<div class="flex shrink-0 flex-col gap-1">
		<Badge variant="outline">Track {track.trackId}</Badge>
		<Badge class="bg-muted text-muted-foreground"><CarIcon /> {track.totalLaps}</Badge>
	</div>

	<div class="flex min-w-0 flex-1 items-center gap-1">
		<PlayerAvatar name={track.player.name} isSmall class="shrink-0" />
		<div class="flex min-w-0 flex-1 flex-col">
			<div class="truncate leading-4">
				{track.player.nickName || track.player.name}
			</div>
			<div class="text-muted-foreground truncate text-xs leading-3">
				{track.player.teamName}
			</div>
		</div>
	</div>

	<Badge class="bg-muted text-muted-foreground shrink-0">
		<FastestIcon class="size-3" />
		{timeSpanToParts(track.fastestLap?.lapTime).value}
	</Badge>
</div>
