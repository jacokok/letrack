<script lang="ts">
	import { tick, type Snippet } from "svelte";
	import Confetti from "svelte-confetti";

	interface Props {
		children?: Snippet;
	}

	let { children }: Props = $props();

	let active = $state(false);

	export const triggerFn = async () => {
		if (active) {
			active = false;
			await tick();
		}
		active = true;
	};
</script>

{#if active}
	{#if children}
		{@render children()}
	{:else}
		<Confetti y={[0.25, 0.5]} x={[-1, 1]} />
	{/if}
{/if}
