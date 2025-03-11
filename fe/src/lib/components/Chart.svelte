<script lang="ts">
	import { BarChart } from "layerchart";

	import { chartData } from "$lib/stores/chart.svelte";
	import { practice } from "$lib/stores/practice.svelte";

	const data = $derived.by(() => {
		const results: Array<{
			lap: number;
			track1: number;
			track2: number;
			track3: number;
			track4: number;
		}> = [];
		let trackIndex = 0;
		for (const trackData of chartData) {
			trackIndex++;
			for (const row of trackData.data) {
				const resultIndex = results.findIndex((r) => r.lap == row.lap);
				if (resultIndex == -1) {
					if (trackIndex == 1) {
						results.push({ lap: row.lap, track1: row.time, track2: 0, track3: 0, track4: 0 });
					}
					if (trackIndex == 2) {
						results.push({ lap: row.lap, track1: 0, track2: row.time, track3: 0, track4: 0 });
					}
					if (trackIndex == 3) {
						results.push({ lap: row.lap, track1: 0, track2: 0, track3: row.time, track4: 0 });
					}
					if (trackIndex == 4) {
						results.push({ lap: row.lap, track1: 0, track2: 0, track3: 0, track4: row.time });
					}
				} else {
					if (trackIndex == 1) {
						results[resultIndex].track1 = row.time;
					}
					if (trackIndex == 2) {
						results[resultIndex].track2 = row.time;
					}
					if (trackIndex == 3) {
						results[resultIndex].track3 = row.time;
					}
					if (trackIndex == 4) {
						results[resultIndex].track4 = row.time;
					}
				}
			}
		}
		return results;
	});

	const series = $derived.by(() => {
		const results = Array<{ key: string; color: string }>();
		if (practice.value.track1 == true) {
			results.push({ key: "track1", color: "hsl(var(--color-primary))" });
		}
		if (practice.value.track2 == true) {
			results.push({ key: "track2", color: "hsl(var(--color-danger))" });
		}
		if (practice.value.track3 == true) {
			results.push({ key: "track3", color: "hsl(var(--color-warning))" });
		}
		if (practice.value.track4 == true) {
			results.push({ key: "track4", color: "hsl(var(--color-secondary))" });
		}
		return results;
	});
</script>

<div class="mt-2 h-[200px] w-full rounded-lg border p-2">
	<BarChart
		{data}
		x="lap"
		axis={true}
		grid={true}
		tooltip={false}
		bandPadding={0.5}
		seriesLayout="group"
		{series}
		props={{ bars: { radius: 2, strokeWidth: 0, tweened: true } }}
	/>
</div>
