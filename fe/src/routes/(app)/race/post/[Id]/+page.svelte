<script lang="ts">
	import { page } from "$app/state";
	import { createLaps, type DTOLapDTO, createLapsValid } from "$lib/api";
	import { timeSpanToParts } from "$lib/util";
	import {
		DataTable,
		createSvelteTable,
		Checkbox,
		Label,
		renderSnippet,
		Badge,
		Tabs,
		Button
	} from "@kayord/ui";
	import FlagIcon from "lucide-svelte/icons/flag";

	import {
		type ColumnDef,
		getCoreRowModel,
		type Updater,
		type PaginationState,
		getPaginationRowModel,
		type RowSelectionState
	} from "@tanstack/table-core";
	import { CheckIcon, XIcon } from "lucide-svelte";

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

	let tab = $state(1);
	const query = $derived(
		createLaps({
			raceId: Number(page.params.Id),
			trackId: tab
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
		},
		{
			accessorKey: "isValid",
			header: "Valid",
			cell: (item) => renderSnippet(valid, item.cell.row.original.isValid)
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
		enableRowSelection: true,
		getRowId: (row) => row.id
	});

	const toggleValidMutation = createLapsValid();
	const toggleValid = async () => {
		await $toggleValidMutation.mutateAsync({
			data: { ids: Object.keys(rowSelection) }
		});
		$query.refetch();
	};
</script>

{#snippet flag(isFlagged: boolean)}
	{#if isFlagged}
		<Badge variant="destructive" class=" animate-pulse">
			<FlagIcon class="size-4" />
		</Badge>
	{/if}
{/snippet}

{#snippet valid(isValid: boolean)}
	{#if isValid}
		<Badge>
			<CheckIcon class="size-4" />
		</Badge>
	{:else}
		<Badge variant="destructive">
			<XIcon class="size-4" />
		</Badge>
	{/if}
{/snippet}

{#snippet header()}
	<div class="flex items-center justify-between">
		<div>
			<h1>Post Race Track {tab}</h1>
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
{/snippet}

<div class="m-2">
	<Tabs.Root value={1} class="flex w-full flex-col" onValueChange={(v: number) => (tab = v)}>
		<Tabs.List class="items-center justify-center text-center">
			<Tabs.Trigger value={1}>Track 1</Tabs.Trigger>
			<Tabs.Trigger value={2}>Track 2</Tabs.Trigger>
		</Tabs.List>
	</Tabs.Root>
	{@render header()}
	<DataTable
		{table}
		{columns}
		headerClass="pb-2"
		isLoading={$query.isPending}
		noDataMessage="No history available"
	/>

	{#if Object.keys(rowSelection).length > 0}
		<Button class="w-full" onclick={toggleValid}>Toggle Is Valid</Button>
	{/if}
</div>
