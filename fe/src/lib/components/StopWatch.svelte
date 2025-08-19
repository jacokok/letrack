<script lang="ts">
	import { Timer } from "$lib/stores/timer.svelte";
	import TimerIcon from "@lucide/svelte/icons/timer";
	import { Card } from "@kayord/ui";

	interface Props {
		timer: Timer;
	}
	let { timer }: Props = $props();
</script>

<Card.Root class="flex flex-row w-72 items-center gap-2 p-2">
	<TimerIcon class="size-10" />
	<h1 class="flex gap-2 text-5xl font-bold tabular-nums">
		{timer.minutesFmt}:{timer.secondsFmt}:{timer.msFmt}
	</h1>
</Card.Root>
