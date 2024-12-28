<script lang="ts">
	import type { RaceSummaryTrack } from "$lib/api";

	import Laps from "$lib/components/Laps.svelte";
	import StopWatch from "$lib/components/StopWatch.svelte";
	import type { DoneEvent, SaveEvent } from "$lib/types";
	import TrackSummary from "./TrackSummary.svelte";

	interface Props {
		track: RaceSummaryTrack;
		refetch: () => void;
	}

	let { track, refetch }: Props = $props();

	let stopWatch: StopWatch;

	export const doneEvent = (evt: DoneEvent) => {
		refetch();
	};

	export const receiveEvent = (evt: SaveEvent) => {
		stopWatch.receiveEvent(evt);
	};
</script>

<div class="flex w-full flex-col gap-2">
	<Laps laps={track.laps} fastestLap={track.fastestLap}>
		<TrackSummary {track} />
		<div class="flex justify-center">
			<StopWatch bind:this={stopWatch} />
		</div>
	</Laps>
</div>
