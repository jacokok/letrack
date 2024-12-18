<script lang="ts">
	import { LineChart } from "layerchart";
	import { curveBumpX } from "d3-shape";

	interface Props {
		data: Array<{ lap: number; time: number }>;
	}

	let { data }: Props = $props();
</script>

<div class="h-[300px] rounded-lg border p-4">
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
