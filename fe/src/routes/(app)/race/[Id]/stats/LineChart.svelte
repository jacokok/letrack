<script lang="ts">
	import type { DTOLapDTO, RaceSummaryTrack } from "$lib/api";
	import { LineChart } from "layerchart";

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
</script>

<div class="h-[500px] p-4 border rounded m-2">
	<LineChart
		x="timestamp"
		data={chartData}
		series={[
			{
				key: "first",
				label: `Track ${firstTrack}`,
				color: firstTrackColor
			},
			{
				key: "second",
				label: `Track ${secondTrack}`,
				color: secondTrackColor
			}
		]}
		axis="y"
		props={{ spline: { strokeWidth: 5, draw: true } }}
		legend
	/>
</div>
