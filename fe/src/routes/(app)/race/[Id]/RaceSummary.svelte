<script lang="ts">
	import type { EntitiesRace, RaceSummaryResponse, RaceSummaryResponseFastestLap } from "$lib/api";
	import CountDownTimer from "$lib/components/CountDownTimer.svelte";
	import { timeSpanToParts } from "$lib/util";
	import { Badge, Card } from "@kayord/ui";
	import TimerIcon from "lucide-svelte/icons/hourglass";

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
			{:else if data.race.timeRemaining != null}
				<Card.Root class="flex w-72 items-center gap-2 p-2">
					<TimerIcon class="size-10" />
					<h1 class="flex gap-2 text-5xl font-bold">
						{data.race.timeRemaining.substring(0, 8)}
					</h1>
				</Card.Root>
			{/if}
		</div>

		<div class="flex flex-col items-end gap-1">
			<div class="bg-muted text-muted-foreground flex items-center gap-2 rounded-xl px-2">
				<span>Total Laps:</span>
				<h1>{data.totalLaps}</h1>
				{#if data.race.endLapCount}
					First to {data.race.endLapCount}
				{/if}
			</div>

			<div class="bg-muted text-muted-foreground flex items-center gap-2 rounded-xl px-2">
				<span>Fastest Lap:</span>
				<h1>{timeSpanToParts(data.fastestLap?.lapTime).value}</h1>
				{fastestLapBy?.name}
			</div>
		</div>
	</div>
</Card.Root>
