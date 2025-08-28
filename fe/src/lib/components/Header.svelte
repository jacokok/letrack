<script lang="ts">
	import { goto } from "$app/navigation";
	import type { Snippet } from "svelte";

	interface Props {
		right?: Snippet;
	}
	let { right }: Props = $props();

	import { hub } from "$lib/stores/hub.svelte";
	import { HubConnectionState } from "@microsoft/signalr";

	const isConnected = $derived(hub.state == HubConnectionState.Connected);
</script>

<div class="flex h-12 w-full items-center justify-between gap-2 bg-muted/30 px-2">
	<div class="flex items-center gap-2">
		<button class="flex items-center gap-2" onclick={() => goto("/")}>
			<img src="/favicon.svg" alt="LeTrack" class="h-8 animate-spin [animation-duration:5s]" />
			<h1 class="text-3xl tracking-tighter">
				<span class="text-secondary">Le</span><span class="font-bold text-primary">Track</span>
			</h1>
		</button>
		<div
			class={`size-2 rounded-full ${isConnected ? "bg-primary" : "bg-destructive  animate-pulse"}`}
		></div>
	</div>
	{@render right?.()}
</div>
