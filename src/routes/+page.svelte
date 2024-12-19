<script lang="ts">
	import { Avatar, Badge, Button, Card, Table } from "@kayord/ui";
	import StopWatch from "$lib/components/StopWatch.svelte";
	import LapsChart from "$lib/components/LapsChart.svelte";
	import FlagIcon from "lucide-svelte/icons/flag";
	import FastestIcon from "lucide-svelte/icons/zap";

	import type { PageData } from "./$types";
	import { invalidate } from "$app/navigation";
	import { timeSpanToParts } from "$lib/util";
	import type { DoneEvent } from "$lib/types";
	import { hub } from "$lib/stores/hub.svelte";
	import Laps from "$lib/components/Laps.svelte";

	let { data }: { data: PageData } = $props();

	const refresh = () => {
		invalidate("track:1");
	};

	const doneEvent = (evt: DoneEvent) => {
		refresh();
	};

	$effect(() => {
		if (hub.state == "Connected") {
			hub.on("DoneEvent", doneEvent);
			return () => {
				hub.off("DoneEvent", doneEvent);
			};
		}
	});

	const chartData = $derived(
		data.trackSummary.last10Laps.map((lap, i) => ({
			lap: i + 1,
			time: timeSpanToParts(lap.lapTime).toSeconds
		}))
	);
</script>

<div class="mb-6 flex items-center bg-muted p-2">
	<div class="flex w-full flex-col items-start pl-2">
		<h1>{data.trackSummary.race?.raceTracks[0].player.name}</h1>
		<Card.Root class="flex items-center gap-2 p-2">
			Fastest Lap: <h1>{timeSpanToParts(data.trackSummary.fastestLap?.lapTime).value}</h1>
		</Card.Root>
	</div>
	<div class="flex w-full flex-col items-end">
		<Card.Root class="flex items-center gap-2 p-2">
			Laps: <h1>{data.trackSummary.totalLaps}</h1>
		</Card.Root>
		<Card.Root class="flex items-center gap-2 p-2">
			Time Remaining: <h1>00:30:34</h1>
		</Card.Root>
	</div>
</div>

<StopWatch />

<div class="flex w-full flex-row">
	<Laps laps={data.trackSummary.last10Laps} fastestLap={data.trackSummary.fastestLap} />
	<Card.Root class="m-2 flex w-full flex-col gap-2 bg-muted/50 p-2 ">
		{#each data.trackSummary.last10Laps as lap}
			{@const diff = timeSpanToParts(lap.lapTimeDifference)}
			<Card.Root class="flex items-center gap-2 p-2">
				<Badge variant="secondary">{lap.lapNumber}</Badge>
				<h1>{timeSpanToParts(lap.lapTime).value}</h1>
				<div class={`${diff.isMinus ? "text-destructive" : "text-green-300"}`}>
					{diff.isMinus ? "-" : "+"}{diff.value}
				</div>
				{#if lap.id == data.trackSummary.fastestLap?.id}
					<Badge class="animate-pulse">
						<FastestIcon class="size-4" />
					</Badge>
				{/if}
				{#if lap.isFlagged}
					<Badge variant="destructive" class=" animate-caret-blink">
						<FlagIcon class="size-4" />
					</Badge>
				{/if}
			</Card.Root>
		{/each}
	</Card.Root>

	<div class="m-2 w-full">
		<LapsChart data={chartData} />
	</div>
</div>
