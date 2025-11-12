<script lang="ts">
	import { page } from "$app/state";
	import {
		createLeaderboardPlayer,
		createPlayersGet,
		type LeaderboardPlayerResponse
	} from "$lib/api";
	import Header from "$lib/components/Header.svelte";
	import { Button, Card } from "@kayord/ui";
	import { Chart } from "@kayord/ui/chart";
	import { aggregationFns, createShadTable, DataTable } from "@kayord/ui/data-table";
	import { type ColumnDef } from "@tanstack/table-core";
	import { scaleBand } from "d3-scale";
	import { BarChart } from "layerchart";
	import { cubicInOut } from "svelte/easing";
	import RefreshIcon from "@lucide/svelte/icons/refresh-cw";

	const playerQuery = createPlayersGet(Number(page.params.Id));
	const player = $derived(playerQuery.data);

	const chartDataQuery = createLeaderboardPlayer(Number(page.params.Id));
	const chartData = $derived(chartDataQuery.data ?? []);

	const chartConfig = {} satisfies Chart.ChartConfig;

	const columns: ColumnDef<LeaderboardPlayerResponse>[] = [
		{
			header: "Race",
			accessorKey: "raceName",
			size: 1000
		},
		{
			header: "Laps",
			accessorKey: "laps",
			size: 1000,
			footer: () => aggregationFns.sum(chartData, "laps")
		}
	];

	const table = createShadTable({
		columns,
		get data() {
			return chartData;
		},
		enableRowSelection: false,
		enablePaging: false
	});
</script>

{#snippet refreshSnippet()}
	<div>
		<Button
			size="icon"
			onclick={() => {
				chartDataQuery.refetch();
				playerQuery.refetch();
			}}
			variant="ghost"
		>
			<RefreshIcon />
		</Button>
	</div>
{/snippet}

<Header right={refreshSnippet} />
<div class="flex flex-col gap-2 m-2">
	<Card.Root>
		<Card.Header>
			<Card.Title>{player?.name} {player?.nickName ? `(${player?.nickName})` : ""}</Card.Title>
			<Card.Description>{player?.team?.name}</Card.Description>
		</Card.Header>
	</Card.Root>
	<div class="flex md:flex-row flex-col w-full gap-2 items-baseline">
		<Card.Root class="w-full">
			<Card.Header>
				<Card.Title>Summary</Card.Title>
				<Card.Description>{player?.name}</Card.Description>
			</Card.Header>
			<Card.Content>
				<DataTable
					headerClass="p-0 m-0"
					{table}
					noDataMessage="No Lap Information"
					class="p-0 m-0"
				/>
			</Card.Content>
		</Card.Root>
		<Card.Root class="w-full">
			<Card.Header>
				<Card.Title>Bar Chart - Mixed</Card.Title>
				<Card.Description>January - June 2024</Card.Description>
			</Card.Header>
			<Card.Content class="min-w-50">
				<Chart.Container config={chartConfig}>
					<BarChart
						data={chartData}
						orientation="horizontal"
						yScale={scaleBand().padding(0.25)}
						y="raceName"
						x="laps"
						c="color"
						padding={{ left: 48 }}
						grid={false}
						rule={false}
						axis="y"
						props={{
							bars: {
								stroke: "none",
								radius: 5,
								rounded: "all",
								initialWidth: 0,
								initialX: 0,
								motion: {
									x: { type: "tween", duration: 500, easing: cubicInOut },
									width: { type: "tween", duration: 500, easing: cubicInOut }
								}
							},
							highlight: { area: { fill: "none" } },
							yAxis: {
								tickLabelProps: {
									svgProps: {
										x: -16
									}
								}
							}
						}}
					>
						{#snippet tooltip()}
							<Chart.Tooltip hideLabel nameKey="visitors" />
						{/snippet}
					</BarChart>
				</Chart.Container>
			</Card.Content>
		</Card.Root>
	</div>
</div>
