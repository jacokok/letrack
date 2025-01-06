<script lang="ts">
	import { BarChart } from "layerchart";

	interface Props {
		data: Array<{ lap: number; time: number }>;
	}

	let { data }: Props = $props();
</script>

<div class="mt-2 h-[68px] w-[300px] rounded-lg border p-2">
	<BarChart
		{data}
		x="lap"
		y="time"
		axis={false}
		grid={false}
		tooltip={false}
		bandPadding={0.1}
		props={{ bars: { radius: 2, strokeWidth: 0, tweened: true } }}
	/>
</div>
