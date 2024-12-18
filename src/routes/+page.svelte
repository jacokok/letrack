<script lang="ts">
	import { Badge, Button, Card, Table } from "@kayord/ui";
	import StopWatch from "$lib/components/StopWatch.svelte";
	import LapsChart from "$lib/components/LapsChart.svelte";

	import type { PageData } from "./$types";
	import { invalidate } from "$app/navigation";
	import { timeSpanToParts } from "$lib/util";
	import type { DoneEvent } from "$lib/types";
	import { hub } from "$lib/stores/hub.svelte";

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

<h1>Lap Time:</h1>
<StopWatch />

<Card.Root class="m-2 p-2">
	<Table.Root>
		<Table.Header>
			<Table.Row>
				<Table.Head>Lap</Table.Head>
			</Table.Row>
		</Table.Header>
		<Table.Body>
			{#each data.trackSummary.last10Laps as lap}
				<Table.Row class="odd:bg-muted">
					<Table.Cell class="flex items-center gap-2">
						{timeSpanToParts(lap.lapTime).value}
						{#if lap.id == data.trackSummary.fastestLap?.id}
							<Badge class="animate-pulse">Fastest</Badge>
						{/if}
						{#if lap.isFlagged}
							<Badge variant="destructive" class="animate-pulse">Flagged</Badge>
						{/if}
					</Table.Cell>
				</Table.Row>
			{/each}
		</Table.Body>
	</Table.Root>
</Card.Root>

<h1>Time Elapsed:</h1>

<div class="m-2">
	<LapsChart data={chartData} />
</div>

{data.trackSummary.fastestLap?.lapTime}

<h1>Total Laps:{data.trackSummary.totalLaps} ({data.trackSummary.laps.length})</h1>
