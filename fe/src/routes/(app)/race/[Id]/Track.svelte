<script lang="ts">
	import type { RaceSummaryTrack } from "$lib/api";

	import Laps from "$lib/components/Laps.svelte";
	import MyConfetti from "$lib/components/MyConfetti.svelte";
	import StopWatch from "$lib/components/StopWatch.svelte";
	import { Timer } from "$lib/stores/timer.svelte";
	import TrackSummary from "./TrackSummary.svelte";

	interface Props {
		track: RaceSummaryTrack;
		timer: Timer;
	}

	let { track, timer }: Props = $props();

	let fastestLapId: string = $state(track.fastestLap?.id ?? "0000000-0000-0000-0000-000000000000");
	let confetti: MyConfetti;

	// Trigger Confetti on Fastest Lap
	$effect(() => {
		if (track.laps.length > 0) {
			if (track.fastestLap?.id != fastestLapId) {
				if (track.fastestLap?.id != undefined && track.laps[0].id == track.fastestLap?.id) {
					confetti.triggerFn();
				}
				if (track.fastestLap?.id != undefined) {
					fastestLapId = track.fastestLap?.id;
				}
			}
		}
	});
</script>

<div class="flex w-full flex-col gap-2">
	<Laps laps={track.laps} fastestLap={track.fastestLap}>
		<TrackSummary {track} />
		<div class="flex flex-col items-center justify-center">
			<StopWatch {timer} />
			<MyConfetti bind:this={confetti} />
		</div>
	</Laps>
</div>
