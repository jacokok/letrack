<script lang="ts">
	import type { EntitiesRace, RaceSummaryResponse, RaceSummaryResponseFastestLap } from "$lib/api";
	import { timeSpanToParts } from "$lib/util";
	import { Badge, Card } from "@kayord/ui";

	interface Props {
		data: RaceSummaryResponse;
	}
	let { data }: Props = $props();

	const fastestLapBy = $derived(
		data.fastestLap ? data.race.raceTracks[data.fastestLap.trackId].player : undefined
	);
</script>

<Card.Root class="m-2 p-2">
	<div class="flex items-center justify-between gap-2">
		<div class="flex items-center gap-2 rounded-xl bg-muted px-2 text-muted-foreground">
			<h1>{data.totalLaps}</h1>
		</div>
		<div class="flex items-center gap-2">
			<h1>{data.race.name}</h1>
			<Badge variant={data.race.isActive ? "default" : "destructive"}>
				{data.race.isActive ? "Active" : "Finished"}
			</Badge>
		</div>
		<div class="flex items-center gap-2 rounded-xl bg-muted px-2 text-muted-foreground">
			<h1>{timeSpanToParts(data.fastestLap?.lapTime).value}</h1>
			{fastestLapBy?.name}
		</div>
	</div>
</Card.Root>
