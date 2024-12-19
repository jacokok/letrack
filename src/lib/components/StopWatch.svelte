<script lang="ts">
	import { Timer } from "$lib/stores/timer.svelte";
	import { hub } from "$lib/stores/hub.svelte";
	import type { SaveEvent } from "$lib/types";
	import TimerIcon from "lucide-svelte/icons/timer";
	import { Card } from "@kayord/ui";

	const timer = new Timer();

	const receiveEvent = (evt: SaveEvent) => {
		timer.start();
	};

	$effect(() => {
		if (hub.state == "Connected") {
			hub.on("ReceiveEvent", receiveEvent);
			return () => {
				hub.off("ReceiveEvent", receiveEvent);
			};
		}
	});
</script>

<Card.Root class="m-2 flex w-fit items-center gap-2 p-2">
	<TimerIcon class="size-10" />
	<h1 class="flex gap-2 text-5xl font-bold">
		{timer.minutesFmt}:{timer.secondsFmt}:{timer.msFmt}
	</h1>
</Card.Root>
