<script lang="ts">
	import { Badge, Button, Tooltip } from "@kayord/ui";
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
	import { cn } from "@kayord/ui/utils";

	let open = $state(false);

	const getVariant = (track: number) => {
		if (track <= 1) {
			return "bg-primary text-primary-foreground";
		} else if (track == 2) {
			return "bg-destructive text-destructive-foreground hover:bg-destructive/80";
		} else if (track == 3) {
			return "bg-accent text-accent-foreground hover:bg-accent/80";
		} else return "bg-secondary text-secondary-foreground hover:bg-secondary/80";
	};

	const columns: ColumnDef<EntitiesRace>[] = [
		{
			header: "Race",
			accessorKey: "name",
			size: 1000
		},
		{
			header: "Slot 1",
			accessorKey: "raceTracks[0]",
			cell: (item) => renderSnippet(track, item.cell.row.original.raceTracks[0]),
			size: 1000,
			enableSorting: false
		},
		{
			header: "Slot 2",
			accessorKey: "raceTracks[1]",
			cell: (item) => renderSnippet(track, item.cell.row.original.raceTracks[1]),
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

	let sorting: SortingState = $state([]);
	const setSorting = (updater: Updater<SortingState>) => {
		if (updater instanceof Function) {
			sorting = updater(sorting);
		} else sorting = updater;
	};
	const sorts = $derived(sorting.map((sort) => `${sort.desc ? "-" : ""}${sort.id}`).join(","));

	const query = $derived(
		createRaceList({ page: pagination.pageIndex + 1, pageSize: pagination.pageSize, sorts })
	);
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
		{track.player.name} ({track.player.nickName})
		<Badge class={cn(getVariant(track.trackId))}>Track {track.trackId}</Badge>
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
						<Badge variant="outline">
							{new Date(race.createdDateTime).toLocaleString()}
						</Badge>
					</p>
					{#if race.startDateTime}
						<p class="flex justify-between gap-2">
							Started:
							<Badge variant="outline">
								{new Date(race.startDateTime).toLocaleString()}
							</Badge>
						</p>
					{/if}
					{#if race.endDateTime}
						<p class="flex justify-between gap-2">
							Ended:
							<Badge variant="outline">
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
		<Button class="my-2" onclick={() => (open = true)}>
			<PlusIcon />Add Race
		</Button>
	</div>
{/snippet}

<div class="m-2">
	<DataTable {header} headerClass="pb-2" {table} noDataMessage="No Races" enableVisibility />
	<AddRace bind:open refetch={query.refetch} />
</div>
