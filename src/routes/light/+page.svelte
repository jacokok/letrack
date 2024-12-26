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

<div class="flex items-center justify-center">
	<Lights bind:isActive />
</div>

<Button onclick={toggleActive}>
	{#if isActive}
		<StopIcon />
	{:else}
		<PlayIcon />
	{/if}
</Button>
