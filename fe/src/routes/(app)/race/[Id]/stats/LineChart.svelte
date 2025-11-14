<script lang="ts">
	import type { DTOLapDTO, RaceSummaryTrack } from "$lib/api";
	import { Card } from "@kayord/ui";
	import { Chart } from "@kayord/ui/chart";
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
		firstTrack == 1 ? "hsl(var(--color-success) / 0.8)" : "hsl(var(--color-warning) / 0.8)"
	);
	const secondTrackColor = $derived(
		secondTrack == 2 ? "hsl(var(--color-danger) / 0.8)" : "hsl(var(--color-secondary) / 0.8)"
	);

	const chartData = $derived.by(() => {
		const laps: DTOLapDTO[] = [];
		data.forEach((d) => laps.push(...d.laps));
		if (laps.length === 0) return [];

		laps.sort((a, b) => new Date(a.timestamp).getTime() - new Date(b.timestamp).getTime());

		const bucketMs = 5000;
		const floorBucket = (t: number) => Math.floor(t / bucketMs) * bucketMs;

		const firstTimestamp = new Date(laps[0].timestamp).getTime();
		const lastTimestamp = new Date(laps[laps.length - 1].timestamp).getTime();
		const minBucket = floorBucket(firstTimestamp);
		const maxBucket = floorBucket(lastTimestamp);

		// record the last known values per bucket (later laps in same bucket override earlier)
		const bucketMap = new Map<number, { first: number; second: number }>();
		let currentFirst = 0;
		let currentSecond = 0;
		for (const lap of laps) {
			const t = new Date(lap.timestamp).getTime();
			const bucket = floorBucket(t);
			if (lap.trackId === 1 || lap.trackId === 3) currentFirst = lap.lapNumber;
			else if (lap.trackId === 2 || lap.trackId === 4) currentSecond = lap.lapNumber;
			bucketMap.set(bucket, { first: currentFirst, second: currentSecond });
		}

		// build a value for every 5s bucket, carrying forward the last known values
		const result: Array<{ timestamp: Date; first: number; second: number }> = [];
		let lastFirst = 0;
		let lastSecond = 0;
		for (let b = minBucket; b <= maxBucket; b += bucketMs) {
			const v = bucketMap.get(b);
			if (v) {
				lastFirst = v.first;
				lastSecond = v.second;
			}
			result.push({ timestamp: new Date(b), first: lastFirst, second: lastSecond });
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
