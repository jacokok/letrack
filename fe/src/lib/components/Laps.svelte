<script lang="ts">
	import type { TrackSummaryResponseFastestLap } from "$lib/api";
	import type { Lap } from "$lib/types";
	import { timeSpanToParts } from "$lib/util";
	import { Alert, Badge, Card, Tooltip } from "@kayord/ui";
	import FlagIcon from "lucide-svelte/icons/flag";
	import FastestIcon from "lucide-svelte/icons/zap";
	import type { Snippet } from "svelte";

	interface Props {
		laps: Array<Lap>;
		fastestLap?: TrackSummaryResponseFastestLap;
		children?: Snippet;
	}

	let { laps, fastestLap, children }: Props = $props();
</script>

<Card.Root class="flex w-full flex-col gap-2 bg-muted/50 p-2 ">
	{@render children?.()}
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
				<Badge class="animate-caret-blink">
					<FastestIcon class="size-4" />
				</Badge>
			{/if}
			{#if lap.isFlagged}
				<Tooltip.Provider>
					<Tooltip.Root>
						<Tooltip.Trigger>
							<Badge variant="destructive" class=" animate-pulse">
								<FlagIcon class="size-4" />
							</Badge>
						</Tooltip.Trigger>
						<Tooltip.Content>
							<p>{lap.flagReason}</p>
						</Tooltip.Content>
					</Tooltip.Root>
				</Tooltip.Provider>
			{/if}
		</Card.Root>
	{/each}
</Card.Root>
