<script lang="ts">
	import type { TrackSummaryResponseFastestLap } from "$lib/api";
	import type { Lap } from "$lib/types";
	import { timeSpanToParts } from "$lib/util";
	import { Alert, Badge, Card } from "@kayord/ui";
	import FlagIcon from "lucide-svelte/icons/flag";
	import FastestIcon from "lucide-svelte/icons/zap";

	interface Props {
		laps: Array<Lap>;
		fastestLap?: TrackSummaryResponseFastestLap;
	}

	let { laps, fastestLap }: Props = $props();
</script>

<Card.Root class="m-2 flex w-full flex-col gap-2 bg-muted/50 p-2 ">
	{#if laps.length <= 0}
		<Alert.Root>
			<Alert.Title>No laps have been recorded yet!</Alert.Title>
			<Alert.Description>Current track have not received any events</Alert.Description>
		</Alert.Root>
	{/if}
	{#each laps as lap}
		{@const diff = timeSpanToParts(lap.lapTimeDifference)}
		<Card.Root class="flex items-center gap-2 p-2">
			<Badge class="bg-muted text-muted-foreground">{lap.lapNumber}</Badge>
			<h1>{timeSpanToParts(lap.lapTime).value}</h1>
			<div class={`${diff.isMinus ? "text-destructive" : "text-green-300"}`}>
				{diff.isMinus ? "-" : "+"}{diff.value}
			</div>
			{#if lap.id == fastestLap?.id}
				<Badge class="animate-pulse">
					<FastestIcon class="size-4" />
				</Badge>
			{/if}
			{#if lap.isFlagged}
				<Badge variant="destructive" class=" animate-caret-blink">
					<FlagIcon class="size-4" />
				</Badge>
			{/if}
		</Card.Root>
	{/each}
</Card.Root>
