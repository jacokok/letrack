<script lang="ts">
	import { createTrackSummary } from "$lib/api";
	import Laps from "$lib/components/Laps.svelte";
	import StopWatch from "$lib/components/StopWatch.svelte";
	import { chartData } from "$lib/stores/chart.svelte";
	import type { DoneEvent, SaveEvent } from "$lib/types";
	import { timeSpanToParts } from "$lib/util";
	import { Alert, Loader } from "@kayord/ui";

	interface Props {
		trackId: number;
	}

	export const doneEvent = (evt: DoneEvent) => {
		$query.refetch();
	};

	export const receiveEvent = (evt: SaveEvent) => {
		console.log("Recieved Event", evt, " on Track", trackId);
		stopWatch.receiveEvent(evt);
	};

	let { trackId }: Props = $props();

	const query = createTrackSummary(trackId);

	let stopWatch: StopWatch;

	$effect(() => {
		if ($query.data) {
			chartData[trackId - 1].data = $query.data.last10Laps.map((lap, i) => ({
				lap: i + 1,
				time: timeSpanToParts(lap.lapTime).toSeconds
			}));
		} else {
			chartData[trackId - 1].data = [];
		}
	});
</script>

<div class="flex w-full flex-col">
	<StopWatch bind:this={stopWatch} />
	{#if $query.error}
		<Alert.Root>
			<Alert.Title>An Error Occurred!</Alert.Title>
			<Alert.Description>{$query.error}</Alert.Description>
		</Alert.Root>
	{/if}

	{#if $query.isPending}
		<Loader />
	{/if}

	{#if $query.data}
		<Laps laps={$query.data.last10Laps ?? []} fastestLap={$query.data.fastestLap} />
	{/if}
</div>
