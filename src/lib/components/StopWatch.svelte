<script lang="ts">
	import { Button } from "@kayord/ui";
	import { Timer } from "$lib/stores/timer.svelte";
	import { hub } from "$lib/stores/hub.svelte";

	const timer = new Timer();

	const receiveMessage = (message: string) => {
		console.log(message);
		timer.start();
	};

	$effect(() => {
		if (hub.state == "Connected") {
			hub.on("ReceiveMessage", receiveMessage);
			return () => {
				hub.off("ReceiveMessage", receiveMessage);
			};
		}
	});
</script>

<div>
	<Button onclick={timer.start}>Start</Button>

	<div class="flex gap-2 text-xl">
		{timer.minutesFmt}:{timer.secondsFmt}:{timer.msFmt}
	</div>

	{timer.lastLap?.toString() ?? "What"}
</div>
