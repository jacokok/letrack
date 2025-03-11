interface ChartRow {
	lap: number;
	time: number;
}

interface ChartData {
	data: Array<ChartRow>;
}

const chartData = $state<Array<ChartData>>([
	{ data: [] },
	{ data: [] },
	{ data: [] },
	{ data: [] }
]);

export { chartData };
