<script lang="ts">
	import type { RaceSummaryTrack } from "$lib/api";

	import Laps from "$lib/components/Laps.svelte";
	import StopWatch from "$lib/components/StopWatch.svelte";
	import { Timer } from "$lib/stores/timer.svelte";
	import type { DoneEvent, SaveEvent } from "$lib/types";
	import TrackSummary from "./TrackSummary.svelte";

	interface Props {
		track: RaceSummaryTrack;
		timer: Timer;
	}

	let { track, timer }: Props = $props();
</script>

<div class="flex w-full flex-col gap-2">
	<Laps laps={track.laps} fastestLap={track.fastestLap}>
		<TrackSummary {track} />
		<div class="flex justify-center">
			<StopWatch {timer} />
		</div>
	</Laps>
</div>
