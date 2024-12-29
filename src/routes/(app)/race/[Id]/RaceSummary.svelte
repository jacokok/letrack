<script lang="ts">
	import type { EntitiesRace, RaceSummaryResponse, RaceSummaryResponseFastestLap } from "$lib/api";
	import CountDownTimer from "$lib/components/CountDownTimer.svelte";
	import { timeSpanToParts } from "$lib/util";
	import { Badge, Card } from "@kayord/ui";

	interface Props {
		data: RaceSummaryResponse;
	}
	let { data }: Props = $props();

	const fastestLapBy = $derived(
		data.fastestLap ? data.race.raceTracks[data.fastestLap.trackId]?.player : undefined
	);
</script>

<Card.Root class="m-2 p-2">
	<div class="flex items-center justify-between gap-2">
		<div class="flex flex-col items-start gap-1">
			<h1>{data.race.name}</h1>
			<Badge variant={data.race.isActive ? "default" : "destructive"}>
				{data.race.isActive ? "Active" : data.totalLaps > 0 ? "Finished" : "Not Started"}
			</Badge>
		</div>

		<div class="flex items-center justify-center">
			{#if data.race.endDateTime && data.race.isActive}
				<CountDownTimer endDateTime={data.race.endDateTime} />
			{/if}
		</div>

		<div class="flex flex-col items-end gap-1">
			<div class="flex items-center gap-2 rounded-xl bg-muted px-2 text-muted-foreground">
				<span>Total Laps:</span>
				<h1>{data.totalLaps}</h1>
				{#if data.race.endLapCount}
					First to {data.race.endLapCount}
				{/if}
			</div>

			<div class="flex items-center gap-2 rounded-xl bg-muted px-2 text-muted-foreground">
				<span>Fastest Lap:</span>
				<h1>{timeSpanToParts(data.fastestLap?.lapTime).value}</h1>
				{fastestLapBy?.name}
			</div>
		</div>
	</div>
</Card.Root>
