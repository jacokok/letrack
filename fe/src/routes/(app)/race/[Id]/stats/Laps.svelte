<script lang="ts">
	import type { DTOLapDTO, RaceSummaryTrack } from "$lib/api";
	import { timeSpanToParts } from "$lib/util";
	import { Badge, Card, DataTable, renderSnippet, Tooltip, createShadTable } from "@kayord/ui";
	import { CircleCheckIcon, CircleXIcon, FlagIcon } from "@lucide/svelte";
	import type { ColumnDef } from "@tanstack/table-core";

	interface Props {
		data: RaceSummaryTrack | undefined;
	}
	let { data }: Props = $props();

	const columns: ColumnDef<DTOLapDTO>[] = [
		{
			header: "Lap",
			accessorKey: "lapNumber"
		},
		{
			header: "Lap Time",
			accessorKey: "lapTime",
			accessorFn: ({ lapTime }) => timeSpanToParts(lapTime).value
		},
		{
			header: "Status",
			accessorKey: "isValid",
			cell: (item) => renderSnippet(lapStatus, item.cell.row.original)
		}
	];

	const table = createShadTable({
		columns,
		get data() {
			return data?.laps ?? [];
		},
		enableRowSelection: false
	});
</script>

{#snippet lapStatus(lap: DTOLapDTO)}
	<div class="flex items-center gap-2">
		{#if lap.isFlagged}
			<Tooltip.Provider>
				<Tooltip.Root>
					<Tooltip.Trigger><FlagIcon class="stroke-destructive" /></Tooltip.Trigger>
					<Tooltip.Content>
						<p>Lap was flagged</p>
						{lap.flagReason}
					</Tooltip.Content>
				</Tooltip.Root>
			</Tooltip.Provider>
		{/if}
		{#if lap.isValid}
			<Tooltip.Provider>
				<Tooltip.Root>
					<Tooltip.Trigger><CircleCheckIcon class="stroke-primary" /></Tooltip.Trigger>
					<Tooltip.Content>
						<p>Lap is valid</p>
						{lap.flagReason}
					</Tooltip.Content>
				</Tooltip.Root>
			</Tooltip.Provider>
		{:else}
			<Tooltip.Provider>
				<Tooltip.Root>
					<Tooltip.Trigger><CircleXIcon class="stroke-destructive" /></Tooltip.Trigger>
					<Tooltip.Content>
						<p>Lap marked as invalid and will not count</p>
						{lap.flagReason}
					</Tooltip.Content>
				</Tooltip.Root>
			</Tooltip.Provider>
		{/if}
	</div>
{/snippet}

<Card.Root class="p-2 w-full">
	<Card.Root class="flex items-center gap-2 bg-black/20 p-2">
		<Badge variant="outline">Track {data?.trackId}</Badge>
		<div>{data?.player.name}</div>
	</Card.Root>
	<DataTable headerClass="pb-2" {table} noDataMessage="No laps" />
</Card.Root>
