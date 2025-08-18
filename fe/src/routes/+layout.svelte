<script lang="ts">
	import { hub } from "$lib/stores/hub.svelte";
	import { QueryClient, QueryClientProvider } from "@tanstack/svelte-query";
	import "../app.css";
	import { browser } from "$app/environment";
	import { Toaster } from "@kayord/ui";
	import { onMount } from "svelte";
	let { children } = $props();

	const receiveMessage = (message: string) => {
		console.log(message);
	};

	onMount(() => {
		hub.init();
		return () => {
			hub.disconnect();
		};
	});

	$effect(() => {
		if (hub.state == "Connected") {
			hub.on("ReceiveMessage", receiveMessage);
			return () => {
				hub.off("ReceiveMessage", receiveMessage);
			};
		}
	});

	const queryClient = new QueryClient({
		defaultOptions: {
			queries: {
				enabled: browser
			}
		}
	});
</script>

<QueryClientProvider client={queryClient}>
	<Toaster />
	{@render children()}
</QueryClientProvider>
