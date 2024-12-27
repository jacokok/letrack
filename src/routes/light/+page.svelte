<script lang="ts">
	import Lights from "$lib/components/Light/Lights.svelte";
	import { Button } from "@kayord/ui";
	import PlayIcon from "lucide-svelte/icons/play";
	import StopIcon from "lucide-svelte/icons/square";

	let isActive = $state(false);

	const toggleActive = () => {
		isActive = !isActive;
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
</div>
