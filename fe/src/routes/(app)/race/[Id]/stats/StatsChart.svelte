<script lang="ts">
	import type { RaceSummaryTrack } from "$lib/api";
	import { trackColors } from "$lib/util";
	import { Card } from "@kayord/ui";
	import { BarChart } from "layerchart";

	interface Props {
		data: RaceSummaryTrack[];
	}
	let { data = [] }: Props = $props();

	const chartData = $derived.by(() => {
		return data.map((t) => {
			return {
				track: `Track ${t.trackId}`,
				laps: t.totalLaps
			};
		});
	});
</script>

<Card.Root class="flex w-full rounded-xl border p-4">
	<div class="h-75 w-full p-2">
		<BarChart
			data={chartData}
			orientation="horizontal"
			x="laps"
			y="track"
			c="track"
			cRange={[trackColors.track1, trackColors.track2, trackColors.track3, trackColors.track4]}
		/>
	</div>
</Card.Root>
