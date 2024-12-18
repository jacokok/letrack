<script lang="ts">
	import { Button, Card } from "@kayord/ui";
	import { CheckCircleIcon, CircleXIcon } from "lucide-svelte";
	import { hub } from "$lib/stores/hub.svelte";
	import StopWatch from "$lib/components/StopWatch.svelte";
	import LapsChart from "$lib/components/LapsChart.svelte";

	import type { PageData } from "./$types";
	import { invalidate, invalidateAll } from "$app/navigation";

	let { data }: { data: PageData } = $props();

	const refresh = () => {
		invalidate("track:1");
		// invalidateAll();
	};
</script>

<h1>Lap Time:</h1>
<StopWatch />

<h1>Time Elapsed:</h1>

<div class="m-2">
	<Card.Root class="mb-2"></Card.Root>

	<LapsChart />
</div>

<div>{JSON.stringify(data)}</div>
<Button onclick={refresh}>Refresh</Button>

{data.data.fastestLap.lapTime}
