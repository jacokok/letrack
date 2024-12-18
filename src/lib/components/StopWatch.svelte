<script lang="ts">
	import { Button } from "@kayord/ui";
	import { Timer } from "$lib/stores/timer.svelte";
	import { hub } from "$lib/stores/hub.svelte";
	import type { SaveEvent } from "$lib/types";

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

<div>
	<h1 class="flex gap-2">
		{timer.minutesFmt}:{timer.secondsFmt}:{timer.msFmt}
	</h1>

	Last lap: {timer.lastLap?.toString()}
</div>
