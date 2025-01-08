<script lang="ts">
	import { page } from "$app/state";
	import { createLaps, type DTOLapDTO } from "$lib/api";
	import { timeSpanToParts } from "$lib/util";
	import { DataTable, createSvelteTable, Checkbox, Label, renderSnippet, Badge } from "@kayord/ui";
	import FlagIcon from "lucide-svelte/icons/flag";

	import {
		type ColumnDef,
		getCoreRowModel,
		type Updater,
		type PaginationState,
		getPaginationRowModel,
		type RowSelectionState
	} from "@tanstack/table-core";

	let showAll = $state<boolean>(false);

	let rowSelection: RowSelectionState = $state({});
	const setRowSelection = (updater: Updater<RowSelectionState>) => {
		if (updater instanceof Function) {
			rowSelection = updater(rowSelection);
		} else rowSelection = updater;
	};

	let pagination: PaginationState = $state({ pageIndex: 0, pageSize: 20 });
	const setPagination = (updater: Updater<PaginationState>) => {
		if (updater instanceof Function) {
			pagination = updater(pagination);
		} else pagination = updater;
	};

	const query = $derived(
		createLaps({
			raceId: Number(page.params.Id),
			trackId: 1
		})
	);

	const data = $derived.by(() => {
		let data = $query.data ?? [];
		if (!showAll) {
			data = data.filter((x) => x.isFlagged == true);
		}
		return data;
	});

	const columns: ColumnDef<DTOLapDTO>[] = [
		{
			accessorKey: "lapNumber",
			header: "Lap"
		},
		{
			header: "Lap Time",
			accessorFn: ({ lapTime }) => timeSpanToParts(lapTime).value
		},
		{
			accessorKey: "isFlagged",
			header: "Flagged",
			cell: (item) => renderSnippet(flag, item.cell.row.original.isFlagged)
		},
		{
			accessorKey: "flagReason",
			header: "Flag Reason"
		}
	];

	const table = createSvelteTable({
		columns,
		get data() {
			return data;
		},
		getCoreRowModel: getCoreRowModel(),
		getPaginationRowModel: getPaginationRowModel(),
		state: {
			get pagination() {
				return pagination;
			},
			get rowSelection() {
				return rowSelection;
			}
		},
		onPaginationChange: setPagination,
		onRowSelectionChange: setRowSelection,
		enableRowSelection: true
	});
</script>

{#snippet flag(isFlagged: boolean)}
	{#if isFlagged}
		<Badge variant="destructive" class=" animate-pulse">
			<FlagIcon class="size-4" />
		</Badge>
	{/if}
{/snippet}

<div class="m-2">
	<div class="flex items-center justify-between">
		<div>
			<h1>Post Race</h1>
			<p class="text-muted-foreground">Check flagged laps</p>
		</div>
		<div class="flex items-center gap-2">
			<Checkbox id="flag" bind:checked={showAll} aria-labelledby="flag-label" />
			<Label
				id="flag-label"
				for="flag"
				class="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70"
			>
				Show All Laps
			</Label>
		</div>
	</div>

	<DataTable
		{table}
		{columns}
		headerClass="pb-2"
		isLoading={$query.isPending}
		noDataMessage="No history available"
	/>
</div>
