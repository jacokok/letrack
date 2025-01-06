<script lang="ts">
	import { goto } from "$app/navigation";
	import type { Snippet } from "svelte";

	interface Props {
		right?: Snippet;
	}
	let { right }: Props = $props();
</script>

<div class="flex h-12 w-full items-center justify-between gap-2 bg-muted/30 px-2">
	<button class="flex items-center gap-2" onclick={() => goto("/")}>
		<img src="/favicon.svg" alt="LeTrack" class="h-8" />
		<h1 class="text-3xl tracking-tighter">
			<span class="text-secondary">Le</span><span class="font-bold text-primary">Track</span>
		</h1>
	</button>
	{@render right?.()}
</div>
