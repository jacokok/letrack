<script lang="ts">
	import { Button, Card } from "@kayord/ui";
	import { CheckCircleIcon, CircleXIcon } from "lucide-svelte";
	import { hub } from "$lib/stores/hub.svelte";
	import StopWatch from "$lib/components/StopWatch.svelte";
	import LapsChart from "$lib/components/LapsChart.svelte";

	const sendMessage = () => {
		// hub.connection.invoke("ReceiveMessage", "Hello");
		hub.connection.invoke("SendMessage", "Hello");
		// hub.connection.invoke("ReceiveMessage", "Hello");
	};
</script>

<h1>Lap Time:</h1>
<StopWatch />

<h1>Time Elapsed:</h1>

<div class="m-2">
	<Card.Root class="mb-2"></Card.Root>

	<LapsChart />
</div>
