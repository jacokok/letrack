<script lang="ts">
	import { BarChart } from "layerchart";

	import { chartData } from "$lib/stores/chart.svelte";

	const data = $derived.by(() => {
		const results: Array<{ lap: number; track1: number; track2: number }> = [];
		let trackIndex = 0;
		for (const trackData of chartData) {
			trackIndex++;
			for (const row of trackData.data) {
				const resultIndex = results.findIndex((r) => r.lap == row.lap);
				if (resultIndex == -1) {
					if (trackIndex == 1) {
						results.push({ lap: row.lap, track1: row.time, track2: 0 });
					}
					if (trackIndex == 2) {
						results.push({ lap: row.lap, track1: 0, track2: row.time });
					}
				} else {
					if (trackIndex == 1) {
						results[resultIndex].track1 = row.time;
					}
					if (trackIndex == 2) {
						results[resultIndex].track2 = row.time;
					}
				}
			}
		}
		return results;
	});

	$inspect(data);
</script>

<div class="mt-2 h-[200px] w-[400px] rounded-lg border p-2">
	<BarChart
		{data}
		x="lap"
		axis={true}
		grid={true}
		tooltip={false}
		bandPadding={0.5}
		seriesLayout="group"
		series={[
			{ key: "track1", color: "hsl(var(--color-danger))" },
			{
				key: "track2",
				color: "hsl(var(--color-warning))"
			}
		]}
		props={{ bars: { radius: 2, strokeWidth: 0, tweened: true } }}
	/>
</div>
