<script lang="ts">
	import { hub } from "$lib/stores/hub.svelte";
	import { QueryClient, QueryClientProvider } from "@tanstack/svelte-query";
	import "../layout.css";
	import favicon from "$lib/assets/favicon.svg";
	import { browser } from "$app/environment";
	import { Toaster } from "@kayord/ui/sonner";
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

<svelte:head><link rel="icon" href={favicon} /></svelte:head>
<QueryClientProvider client={queryClient}>
	<Toaster />
	{@render children()}
</QueryClientProvider>
