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

	interface Props {
		trackId: number;
	}

	let confetti: MyConfetti;

	// eslint-disable-next-line @typescript-eslint/no-unused-vars
	export const doneEvent = (evt: DoneEvent) => {
		query.refetch();
	};

	const timer = new Timer();

	// eslint-disable-next-line @typescript-eslint/no-unused-vars
	export const receiveEvent = (evt: SaveEvent) => {
		timer.start();
	};

	let { trackId }: Props = $props();

	const query = createTrackSummary(trackId);

	let fastestLapId: string = $state(
		query.data?.fastestLap?.id ?? "0000000-0000-0000-0000-000000000000"
	);

	$effect(() => {
		if (query.data) {
			chartData[trackId - 1].data = query.data.last10Laps.map((lap, i) => ({
				lap: i + 1,
				time: timeSpanToParts(lap.lapTime).toSeconds
			}));
		} else {
			chartData[trackId - 1].data = [];
		}
	});

	// Trigger Confetti on Fastest Lap
	$effect(() => {
		if (query.data) {
			if (query.data.fastestLap?.id != fastestLapId) {
				if (
					query.data.fastestLap?.id != undefined &&
					query.data.last10Laps.length > 0 &&
					query.data.last10Laps[0].id == query.data.fastestLap?.id
				) {
					confetti.triggerFn();
				}
				if (query.data.fastestLap?.id != undefined) {
					fastestLapId = query.data.fastestLap?.id;
				}
			}
		}
	});

	const practiceColor = () => {
		if (trackId == 1) {
			return "text-[var(--color-primary)]";
		}
		if (trackId == 2) {
			return "text-[hsl(var(--color-danger))]";
		}
		if (trackId == 3) {
			return "text-[hsl(var(--color-warning))]";
		}
		if (trackId == 4) {
			return "text-[hsl(var(--color-secondary))]";
		}
		return "";
	};
</script>

<div class="flex w-full flex-col gap-1">
	<div class="flex flex-col items-center justify-start w-full">
		<StopWatch {timer} colorClass={practiceColor()} />
		<MyConfetti bind:this={confetti} />
	</div>

	{#if query.error}
		<Alert.Root>
			<Alert.Title>An Error Occurred!</Alert.Title>
			<Alert.Description>{query.error}</Alert.Description>
		</Alert.Root>
	{/if}

	{#if query.isPending}
		<Loader />
	{/if}

	{#if query.data}
		<Laps laps={query.data.last10Laps ?? []} fastestLap={query.data.fastestLap} />
	{/if}
</div>
