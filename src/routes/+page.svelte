<script lang="ts">
	import { Button } from "@kayord/ui";
	import { CheckCircleIcon, CircleXIcon } from "lucide-svelte";
	import { hub } from "$lib/stores/hub.svelte";

	const sendMessage = () => {
		// hub.connection.invoke("ReceiveMessage", "Hello");
		hub.connection.invoke("newMessage", "Hello");
		// hub.connection.invoke("ReceiveMessage", "Hello");
	};
</script>

<h1>Welcome to SvelteKit</h1>
<p>Visit <a href="https://svelte.dev/docs/kit">svelte.dev/docs/kit</a> to read the documentation</p>

<Button onclick={sendMessage}>It works?</Button>

<!-- {#if hub.state == "Connected"}
	<CheckCircleIcon class="text-green-400" />
{:else}
	<CircleXIcon class="text-red-400" />
{/if} -->
