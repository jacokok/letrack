<script lang="ts">
	import { Timer } from "$lib/stores/timer.svelte";
	import type { SaveEvent } from "$lib/types";
	import TimerIcon from "@lucide/svelte/icons/timer";
	import { Card } from "@kayord/ui";

	const timer = new Timer();

	export const receiveEvent = (evt: SaveEvent) => {
		timer.start();
	};

	export const stopEvent = () => {
		timer.stop();
	};
</script>

<Card.Root class="flex w-72 items-center gap-2 p-2">
	<TimerIcon class="size-10" />
	<h1 class="flex gap-2 text-5xl font-bold">
		{timer.minutesFmt}:{timer.secondsFmt}:{timer.msFmt}
	</h1>
</Card.Root>
