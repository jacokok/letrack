<script lang="ts">
	// import {
	// 	HubConnection,
	// 	HubConnectionBuilder,
	// 	LogLevel,
	// 	HubConnectionState
	// } from "@microsoft/signalr";
	// import { PUBLIC_API_URL } from "$env/static/public";
	import { hub } from "$lib/stores/hub.svelte";
	import { QueryClient, QueryClientProvider } from "@tanstack/svelte-query";
	import "../app.css";
	import { browser } from "$app/environment";
	import { goto } from "$app/navigation";
	import { Toaster } from "@kayord/ui";
	let { children } = $props();

	const receiveMessage = (message: string) => {
		console.log(message);
	};

	$effect(() => {
		hub.init();
		return () => {
			hub.disconnect();
		};
	});

	// $effect(() => {
	// 	const connection = new HubConnectionBuilder()
	// 		.withUrl(`${PUBLIC_API_URL}/hub`, {
	// 			withCredentials: false
	// 		})
	// 		.withAutomaticReconnect()
	// 		.configureLogging(LogLevel.None)
	// 		.build();

	// 	connection.on("ReceiveMessage", receiveMessage);
	// 	// const connection = new HubConnectionBuilder().withUrl(`${PUBLIC_API_URL}/hub`).build();

	// 	const start = async () => {
	// 		await connection.start();
	// 	};
	// 	start();
	// });

	// $effect(() => {
	// 	hub.init();
	// 	return () => {
	// 		hub.disconnect();
	// 	};
	// });

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
