<script lang="ts">
	import { Timer } from "$lib/stores/timer.svelte";
	import TimerIcon from "@lucide/svelte/icons/timer";
	import { Card } from "@kayord/ui";
	import { cn } from "@kayord/ui/utils";

	interface Props {
		timer: Timer;
		colorClass?: string;
	}
	let { timer, colorClass }: Props = $props();
</script>

<Card.Root class="flex w-72 flex-row items-center gap-1 p-1">
	<TimerIcon class={cn("size-10", colorClass)} />
	<h1 class="flex gap-2 text-5xl font-bold tabular-nums">
		{timer.minutesFmt}:{timer.secondsFmt}:{timer.msFmt}
	</h1>
</Card.Root>
