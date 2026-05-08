<script lang="ts">
	import type { RaceSummaryResponse } from "$lib/api";
	import CountDownTimer from "$lib/components/CountDownTimer.svelte";
	import { Badge, Card } from "@kayord/ui";
	import TimerIcon from "@lucide/svelte/icons/hourglass";

	interface Props {
		data: RaceSummaryResponse;
	}
	let { data }: Props = $props();
</script>

<div class="m-0 p-2">
	<div class="flex items-center justify-between gap-2">
		<div class="flex flex-col items-start gap-1">
			<Badge variant="outline">{data.race.name}</Badge>
		</div>

		<div class="flex items-center justify-center">
			{#if data.race.endDateTime && data.race.isActive}
				<CountDownTimer endDateTime={data.race.endDateTime} />
			{:else if data.race.timeRemaining != null}
				<Card.Root class="flex w-72 flex-row items-center gap-2 p-2">
					<TimerIcon class="size-10" />
					<h1 class="flex gap-2 text-5xl font-bold">
						{data.race.timeRemaining.substring(0, 8)}
					</h1>
				</Card.Root>
			{/if}
		</div>

		<div class="flex flex-col items-end gap-1">
			<Badge variant={data.race.isActive ? "default" : "destructive"}>
				{data.race.isActive ? "Active" : "Stopped"}
			</Badge>
		</div>
	</div>
</div>
