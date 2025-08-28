<script lang="ts">
	import type { DTOLapDTO, RaceSummaryTrack } from "$lib/api";
	import { Card, Chart } from "@kayord/ui";
	import { scaleUtc } from "d3-scale";
	import { LineChart } from "layerchart";
	import { curveBasis } from "d3-shape";

	interface Props {
		data: RaceSummaryTrack[];
	}
	let { data }: Props = $props();

	const firstTrack = $derived.by(() =>
		data.length > 0 && data[0].laps.length > 0 ? data[0].laps[0].trackId : 0
	);
	const secondTrack = $derived.by(() =>
		data.length > 1 && data[1].laps.length > 0 ? data[1].laps[0].trackId : 0
	);
	const firstTrackColor = $derived(
		firstTrack == 1 ? "hsl(var(--color-success))" : "hsl(var(--color-warning))"
	);
	const secondTrackColor = $derived(
		secondTrack == 2 ? "hsl(var(--color-danger))" : "hsl(var(--color-secondary))"
	);

	const chartData = $derived.by(() => {
		const laps: Array<DTOLapDTO> = [];
		data.map((d) => laps.push(...d.laps));
		laps.sort((x, y) => new Date(x.timestamp).getTime() - new Date(y.timestamp).getTime());
		const result: Array<{ timestamp: Date; first: number; second: number }> = [];
		let first = 0;
		let second = 0;
		for (const lap of laps) {
			if (lap.trackId == 1 || lap.trackId == 3) {
				first = lap.lapNumber;
			} else if (lap.trackId == 2 || lap.trackId == 4) {
				second = lap.lapNumber;
			}
			result.push({ timestamp: new Date(lap.timestamp), first, second });
		}
		return result;
	});

	const chartConfig = $derived({
		first: { label: `Track ${firstTrack}`, color: firstTrackColor },
		second: { label: `Track ${secondTrack}`, color: secondTrackColor }
	}) satisfies Chart.ChartConfig;
</script>

<Card.Root class="p-4 border rounded-xl w-full flex">
	<Chart.Container config={chartConfig} class="h-[300px] w-full">
		<LineChart
			data={chartData}
			x="timestamp"
			xScale={scaleUtc()}
			axis="y"
			series={[
				{
					key: "first",
					label: `Track ${firstTrack}`,
					color: chartConfig.first.color
				},
				{
					key: "second",
					label: `Track ${secondTrack}`,
					color: chartConfig.second.color
				}
			]}
			props={{
				spline: { curve: curveBasis, motion: "tween", strokeWidth: 3, draw: true }
			}}
			legend
		>
			{#snippet tooltip()}
				<Chart.Tooltip hideLabel />
			{/snippet}
		</LineChart>
	</Chart.Container>
</Card.Root>
