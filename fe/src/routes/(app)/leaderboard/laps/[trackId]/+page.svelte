<script lang="ts">
	import { page } from "$app/state";
	import { createLeaderboardLap, type LeaderboardFastestLap } from "$lib/api";
	import Header from "$lib/components/Header.svelte";
	import { Button, Card } from "@kayord/ui";
	import { createShadTable, DataTable } from "@kayord/ui/data-table";
	import { type ColumnDef } from "@tanstack/table-core";
	import RefreshIcon from "@lucide/svelte/icons/refresh-cw";

	const query = createLeaderboardLap(Number(page.params.trackId));
	const data = $derived(query.data ?? []);

	const columns: ColumnDef<LeaderboardFastestLap>[] = [
		{
			header: "Lap Time",
			accessorKey: "lapTime",
			size: 1000
		},
		{
			header: "Player",
			accessorFn: (row) => row.name + (row.nickName ? ` (${row.nickName})` : ""),
			size: 1000
		}
	];

	const table = createShadTable({
		columns,
		get data() {
			return data;
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
				query.refetch();
			}}
			variant="ghost"
		>
			<RefreshIcon />
		</Button>
	</div>
{/snippet}

<Header right={refreshSnippet} />
<div class="flex flex-col gap-2 m-2">
	<div class="flex md:flex-row flex-col w-full gap-2 items-baseline">
		<Card.Root class="w-full">
			<Card.Header>
				<Card.Title>Top 10 Fastest Laps</Card.Title>
				<Card.Description>Track {page.params.trackId}</Card.Description>
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
	</div>
</div>
