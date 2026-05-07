<script lang="ts">
	import { Badge, Button, Label, Switch, Tooltip } from "@kayord/ui";
	import {
		DataTable,
		createShadTable,
		renderComponent,
		renderSnippet
	} from "@kayord/ui/data-table";
	import PlusIcon from "@lucide/svelte/icons/plus";
	import { createRaceList, type EntitiesRace, type EntitiesRaceTrack } from "$lib/api";
	import AddRace from "./AddRace.svelte";
	import type { ColumnDef, PaginationState, SortingState, Updater } from "@tanstack/table-core";
	import Actions from "./Actions.svelte";
	import PlayerAvatar from "$lib/components/PlayerAvatar.svelte";

	let open = $state(false);

	const columns: ColumnDef<EntitiesRace>[] = [
		{
			header: "Race",
			accessorKey: "name",
			size: 1000
		},
		{
			header: "Track 1",
			accessorKey: "raceTracks[0]",
			cell: (item) => renderSnippet(track, item.cell.row.original.raceTracks[0]),
			size: 1000,
			enableSorting: false
		},
		{
			header: "Track 2",
			accessorKey: "raceTracks[1]",
			cell: (item) => renderSnippet(track, item.cell.row.original.raceTracks[1]),
			size: 1000,
			enableSorting: false
		},
		{
			header: "Track 3",
			accessorKey: "raceTracks[2]",
			cell: (item) => renderSnippet(track, item.cell.row.original.raceTracks[2]),
			size: 1000,
			enableSorting: false
		},
		{
			header: "Track 4",
			accessorKey: "raceTracks[3]",
			cell: (item) => renderSnippet(track, item.cell.row.original.raceTracks[3]),
			size: 1000,
			enableSorting: false
		},
		{
			header: "Status",
			accessorKey: "endDateTime",
			cell: (item) => renderSnippet(dates, item.cell.row.original),
			size: 1000
		},
		{
			header: "",
			accessorKey: "id",
			size: 10,
			enableSorting: false,
			cell: (item) =>
				renderComponent(Actions, { race: item.cell.row.original, refetch: query.refetch })
		}
	];

	let pagination: PaginationState = $state({ pageIndex: 0, pageSize: 20 });
	const setPagination = (updater: Updater<PaginationState>) => {
		if (updater instanceof Function) {
			pagination = updater(pagination);
		} else pagination = updater;
	};

	let sorting: SortingState = $state([{ id: "name", desc: false }]);
	const setSorting = (updater: Updater<SortingState>) => {
		if (updater instanceof Function) {
			sorting = updater(sorting);
		} else sorting = updater;
	};
	const sorts = $derived(sorting.map((sort) => `${sort.desc ? "-" : ""}${sort.id}`).join(","));

	let hideEndedRaces = $state(false);
	const filters = $derived(hideEndedRaces ? `EndDateTime == null` : undefined);

	const query = createRaceList(() => ({
		page: pagination.pageIndex + 1,
		pageSize: pagination.pageSize,
		sorts,
		filters
	}));

	const data = $derived(query.data?.items ?? []);
	let rowCount = $derived(query.data?.totalCount ?? 0);

	const table = createShadTable({
		columns,
		get data() {
			return data;
		},
		enableRowSelection: false,
		manualPagination: true,
		manualSorting: true,
		enableVisibility: true,
		state: {
			get pagination() {
				return pagination;
			},
			get sorting() {
				return sorting;
			}
		},
		get rowCount() {
			return rowCount;
		},
		onPaginationChange: setPagination,
		onSortingChange: setSorting
	});
</script>

{#snippet track(track: EntitiesRaceTrack)}
	<div class="flex items-center gap-2">
		<PlayerAvatar name={track.player.name} isSmall />
		<div class="flex flex-col">
			<div>
				{track.player.nickName || track.player.name}
			</div>
			<div class="text-muted-foreground text-xs">
				{track.player.team?.name}
			</div>
		</div>
	</div>
{/snippet}

{#snippet dates(race: EntitiesRace)}
	<div class="flex items-center gap-2">
		<Tooltip.Provider>
			<Tooltip.Root>
				<Tooltip.Trigger>
					{#if race.isActive}
						<Badge>Active</Badge>
					{:else if race.endDateTime}
						<Badge variant="destructive">Ended</Badge>
					{:else}
						<Badge variant="outline">Not Started</Badge>
					{/if}
				</Tooltip.Trigger>
				<Tooltip.Content class="flex flex-col gap-1">
					<p class="flex justify-between gap-2">
						Created:
						<Badge variant="default">
							{new Date(race.createdDateTime).toLocaleString()}
						</Badge>
					</p>
					{#if race.startDateTime}
						<p class="flex justify-between gap-2">
							Started:
							<Badge variant="secondary">
								{new Date(race.startDateTime).toLocaleString()}
							</Badge>
						</p>
					{/if}
					{#if race.endDateTime}
						<p class="flex justify-between gap-2">
							Ended:
							<Badge variant="destructive">
								{new Date(race.endDateTime).toLocaleString()}
							</Badge>
						</p>
					{/if}
				</Tooltip.Content>
			</Tooltip.Root>
		</Tooltip.Provider>
	</div>
{/snippet}

{#snippet header()}
	<div class="flex items-center justify-between gap-2">
		<h1 class="text-2xl">Races</h1>
		<div class="flex items-center space-x-2">
			<div class="flex items-center space-x-2">
				<Switch id="hideEnded" bind:checked={hideEndedRaces} />
				<Label for="hideEnded">Hide Ended</Label>
			</div>
			<Button class="my-2" onclick={() => (open = true)}>
				<PlusIcon />Add Race
			</Button>
		</div>
	</div>
{/snippet}

<div class="m-2">
	<DataTable {header} headerClass="pb-2" {table} noDataMessage="No Races" enableVisibility />
	<AddRace bind:open refetch={query.refetch} />
</div>
