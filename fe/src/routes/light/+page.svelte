<script lang="ts">
	import Lights from "$lib/components/Light/Lights.svelte";
	import { Button } from "@kayord/ui";
	import PlayIcon from "@lucide/svelte/icons/play";
	import StopIcon from "@lucide/svelte/icons/square";

	let isActive = $state(false);

	const toggleActive = () => {
		isActive = !isActive;
	};

	const audio = new Audio("/sounds/light.mp3");
	const audio2 = new Audio("/sounds/voice.mp3");
	const playSound = () => {
		audio.play();
	};
	const playSound2 = () => {
		audio2.play();
	};
</script>

<div class="flex h-screen w-full flex-col items-center justify-center gap-4">
	<Lights bind:isActive />

	<Button onclick={toggleActive}>
		{#if isActive}
			<StopIcon />
		{:else}
			<PlayIcon />
		{/if}
	</Button>
	<Button onclick={playSound}>Sound</Button>
	<Button onclick={playSound2}>Sound2</Button>
</div>
