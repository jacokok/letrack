<script lang="ts">
	// import {
	// 	HubConnection,
	// 	HubConnectionBuilder,
	// 	LogLevel,
	// 	HubConnectionState
	// } from "@microsoft/signalr";
	// import { PUBLIC_API_URL } from "$env/static/public";
	import { hub } from "$lib/stores/hub.svelte";
	import "../app.css";
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
</script>

{@render children()}
