<script lang="ts">
	import { page } from "$app/state";
	import { createLaps, type DTOLapDTO, createLapsValid, createRaceSummary } from "$lib/api";
	import { timeSpanToParts } from "$lib/util";
	import { Label, Badge, Tabs, Button, Card, Switch } from "@kayord/ui";
	import { DataTable, createShadTable, renderSnippet } from "@kayord/ui/data-table";
	import FlagIcon from "@lucide/svelte/icons/flag";
	import { watch } from "runed";

	import {
		type ColumnDef,
		getCoreRowModel,
		type Updater,
		type PaginationState,
		getPaginationRowModel,
		type RowSelectionState
	} from "@tanstack/table-core";
	import { CheckIcon, PlusIcon, ToggleRightIcon, XIcon } from "@lucide/svelte";
	import PlayerAvatar from "$lib/components/PlayerAvatar.svelte";
	import Header from "$lib/components/Header.svelte";
	import AddAdjustment from "./AddAdjustment.svelte";

	let showAll = $state(false);
	let adjustmentOpen = $state(false);

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

	const querySummary = createRaceSummary(Number(page.params.Id));

	const tracksDetail = $derived(querySummary.data?.tracks ?? []);
	const tracks = $derived(tracksDetail.map((x) => x.trackId));

	let tab = $state(0);

	const selectedTrackDetail = $derived(tracksDetail.find((x) => x.trackId == tab));
	const selectedLapTotal = $derived(
		selectedTrackDetail?.laps.map((x) => x.isValid).filter((x) => x == true).length
	);

	// Make sure you do not validate other track's laps when switching tabs
	watch(
		() => tab,
		(curr, prev) => {
			if ((prev ?? 0) > 0) {
				rowSelection = {};
			}
		}
	);

	$effect(() => {
		if (tab == 0) {
			tab = tracks.length > 0 ? tracks[0] : 0;
		}
	});
	const query = $derived(
		createLaps({
			raceId: Number(page.params.Id),
			trackId: tab
		})
	);

	const data = $derived.by(() => {
		let data = query.data ?? [];
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
			cell: (item) => renderSnippet(flag, { isFlagged: item.cell.row.original.isFlagged })
		},
		{
			accessorKey: "flagReason",
			header: "Flag Reason"
		},
		{
			accessorKey: "isValid",
			header: "Valid",
			cell: (item) => renderSnippet(valid, { isValid: item.cell.row.original.isValid })
		}
	];

	const table = createShadTable({
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
		await toggleValidMutation.mutateAsync({
			data: { ids: Object.keys(rowSelection) }
		});
		query.refetch();
		querySummary.refetch();
	};
</script>

{#snippet flag(props: { isFlagged: boolean })}
	{#if props.isFlagged}
		<Badge variant="destructive" class=" animate-pulse">
			<FlagIcon class="size-4" />
		</Badge>
	{/if}
{/snippet}

{#snippet valid(props: { isValid: boolean })}
	{#if props.isValid}
		<Badge>
			<CheckIcon class="size-4" />
		</Badge>
	{:else}
		<Badge variant="destructive">
			<XIcon class="size-4" />
		</Badge>
	{/if}
{/snippet}

{#snippet right()}
	<div class="flex items-center gap-2">
		<Button variant="ghost" href="/race/{page.params.Id}"><FlagIcon /> Race</Button>
	</div>
{/snippet}

<Header {right} />

{#snippet header()}
	<Card.Root class="p-2 my-2 flex flex-row gap-2 items-center bg-black/20 justify-between">
		{#if selectedTrackDetail}
			<div class="flex items-center gap-2">
				<PlayerAvatar name={selectedTrackDetail.player.name} isSmall />
				<div class="flex flex-col">
					<div class="leading-4">
						{selectedTrackDetail.player.name}
					</div>
					<div class="text-muted-foreground text-xs leading-3">
						{selectedTrackDetail.player.nickName}
					</div>
				</div>
			</div>
		{/if}
		<div>
			<Badge>Laps: {selectedLapTotal} of {selectedTrackDetail?.laps.length}</Badge>
			<Badge variant="outline">Track {tab}</Badge>
		</div>
	</Card.Root>

	<div class="flex items-center justify-between">
		<div class="flex gap-2 items-center">
			<p class="text-muted-foreground">Check flagged laps</p>
		</div>

		<div class="flex items-center gap-2">
			{#if adjustmentOpen}
				<AddAdjustment
					trackId={tab}
					raceId={Number(page.params.Id)}
					refetch={() => {
						query.refetch();
						querySummary.refetch();
					}}
					bind:open={adjustmentOpen}
				/>
			{/if}
			{@render toggleValidSnippet()}
			<Button size="sm" variant="outline" onclick={() => (adjustmentOpen = true)}>
				<PlusIcon /> Adjustment
			</Button>
			<Switch id="flag" bind:checked={showAll} aria-labelledby="flag-label" />
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
	<Tabs.Root
		value={tab.toString()}
		class="flex w-full flex-col"
		onValueChange={(v: string) => (tab = Number(v))}
	>
		<Tabs.List class="items-center justify-center text-center">
			{#each tracks as trackId (trackId)}
				<Tabs.Trigger value={trackId.toString()}>Track {trackId}</Tabs.Trigger>
			{/each}
		</Tabs.List>
	</Tabs.Root>
	{@render header()}
	<DataTable
		{table}
		headerClass="pb-2"
		isLoading={query.isPending}
		noDataMessage="No history available"
	/>
	{@render toggleValidSnippet()}
</div>

{#snippet toggleValidSnippet()}
	{#if Object.keys(rowSelection).length > 0}
		<Button size="sm" onclick={toggleValid}>
			<ToggleRightIcon />
			Toggle Is Valid
		</Button>
	{/if}
{/snippet}
