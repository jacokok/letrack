<script lang="ts">
	import Lights from "$lib/components/Light/Lights.svelte";
	import { Button } from "@kayord/ui";

	let lights: Lights;

	const startRace = () => {
		lights.start();
	};
</script>

<div class="m-2 flex items-center justify-center">
	<Lights bind:this={lights} />
</div>

<Button onclick={startRace}>Start</Button>
