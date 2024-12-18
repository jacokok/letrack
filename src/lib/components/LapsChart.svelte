<script lang="ts">
	import { BarChart, LineChart } from "layerchart";
	import { curveBumpX } from "d3-shape";

	interface Props {
		data: Array<{ lap: number; time: number }>;
		data2: Array<{ lap: number; time: number }>;
	}

	let { data, data2 }: Props = $props();
</script>

<div class="h-[200px] rounded-lg border p-4">
	<LineChart
		{data}
		x="lap"
		y="time"
		points
		grid={false}
		labels={{ offset: 10 }}
		props={{
			spline: {
				curve: curveBumpX,
				tweened: true
			}
		}}
		series={[
			{
				key: "time",
				color: "hsl(var(--color-primary))",
				label: "Time(s)",
				props: { inset: 16 }
			}
		]}
		axis={false}
	/>
</div>

<div class="mt-2 h-[200px] rounded-lg border p-4">
	<BarChart
		data={data2}
		x="lap"
		y="time"
		axis={false}
		grid={false}
		tooltip={false}
		bandPadding={0.1}
		props={{ bars: { radius: 2, strokeWidth: 0, tweened: true } }}
	/>
</div>
