<script lang="ts">
	import { createTrackSummary } from "$lib/api";
	import Laps from "$lib/components/Laps.svelte";
	import MyConfetti from "$lib/components/MyConfetti.svelte";
	import StopWatch from "$lib/components/StopWatch.svelte";
	import { chartData } from "$lib/stores/chart.svelte";
	import { Timer } from "$lib/stores/timer.svelte";
	import type { DoneEvent, SaveEvent } from "$lib/types";
	import { timeSpanToParts } from "$lib/util";
	import { Alert, Loader } from "@kayord/ui";
	import Confetti from "svelte-confetti";

	interface Props {
		trackId: number;
	}

	let confetti: MyConfetti;

	export const doneEvent = (evt: DoneEvent) => {
		$query.refetch();
	};

	const timer = new Timer();

	export const receiveEvent = (evt: SaveEvent) => {
		timer.start();
	};

	let { trackId }: Props = $props();

	let fastestLapId: string | undefined = $state();

	const query = createTrackSummary(trackId);

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

	// Trigger Confetti on Fastest Lap
	$effect(() => {
		if ($query.data) {
			if ($query.data.fastestLap?.id != fastestLapId) {
				if (
					fastestLapId != undefined &&
					$query.data.fastestLap?.id != undefined &&
					$query.data.last10Laps[0].id == $query.data.fastestLap?.id
				) {
					confetti.triggerFn();
				}
				if ($query.data.fastestLap?.id != undefined) {
					fastestLapId = $query.data.fastestLap?.id;
				}
			}
		}
	});
</script>

<div class="flex w-full flex-col gap-1">
	<div class="flex flex-col items-center justify-center w-full h-full">
		<StopWatch {timer} />
		<MyConfetti bind:this={confetti} />
	</div>

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
