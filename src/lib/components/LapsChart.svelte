<script lang="ts">
	import { LineChart, Axis } from "layerchart";
	import { curveBumpX } from "d3-shape";

	const data = [
		{
			lap: 1,
			time: 60
		},
		{
			lap: 2,
			time: 68
		},
		{
			lap: 3,
			time: 52
		},
		{
			lap: 4,
			time: 60
		},
		{
			lap: 5,
			time: 68
		},
		{
			lap: 6,
			time: 52
		},
		{
			lap: 7,
			time: 60
		},
		{
			lap: 8,
			time: 68
		},
		{
			lap: 9,
			time: 52
		},
		{
			lap: 10,
			time: 60
		},
		{
			lap: 11,
			time: 68
		},
		{
			lap: 12,
			time: 52
		},
		{
			lap: 13,
			time: 60
		},
		{
			lap: 14,
			time: 68
		},
		{
			lap: 15,
			time: 52
		}
	];
</script>

<div class="h-[300px] rounded-lg border p-4">
	<LineChart
		{data}
		x="lap"
		y="time"
		points
		labels={{ offset: 10 }}
		props={{
			spline: {
				curve: curveBumpX,
				draw: true
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
		axis={{
			placement: "bottom",
			label: "Lap"
		}}
	/>
</div>
