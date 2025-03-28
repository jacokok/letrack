<script lang="ts">
	import { createPlayersList, type EntitiesPlayer } from "$lib/api";
	import { Button, Card, DataTable, renderComponent, ShadTable } from "@kayord/ui";
	import PlusIcon from "@lucide/svelte/icons/plus";
	import AddPlayer from "./AddPlayer.svelte";
	import Actions from "./Actions.svelte";
	import PlayerAvatar from "$lib/components/PlayerAvatar.svelte";
	import type { ColumnDef } from "@tanstack/table-core";

	const query = createPlayersList();

	let open = $state(false);

	const columns: ColumnDef<EntitiesPlayer>[] = [
		{
			header: "",
			accessorKey: "name",
			enableSorting: false,
			size: 10,
			cell: ({ getValue }) =>
				renderComponent(PlayerAvatar, { name: getValue<string>(), isSmall: true })
		},
		{
			header: "Name",
			accessorKey: "name",
			size: 1000
		},
		{
			header: "Nickname",
			accessorKey: "nickName",
			size: 1000
		},
		{
			header: "Team",
			accessorKey: "team.name",
			size: 1000
		},
		{
			header: "",
			accessorKey: "id",
			size: 10,
			enableSorting: false,
			cell: (item) =>
				renderComponent(Actions, { player: item.cell.row.original, refetch: $query.refetch })
		}
	];

	const data = $derived($query.data ?? []);

	const tableState = new ShadTable({
		columns,
		get data() {
			return data;
		},
		enableRowSelection: false
	});
</script>

{#snippet header()}
	<div class="flex justify-between items-center">
		<h2 class="text-2xl">Players</h2>
		<Button class="my-2" onclick={() => (open = true)}>
			<PlusIcon />Add new Player
		</Button>
	</div>
{/snippet}

<div class="m-2">
	<DataTable {header} headerClass="pb-2" {tableState} noDataMessage="No Players" />
	<AddPlayer bind:open refetch={$query.refetch} />
</div>
