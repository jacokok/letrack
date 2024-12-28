<script lang="ts">
	import { page } from "$app/state";
	import { createRaceSummary, type RaceSummaryTrack } from "$lib/api";
	import Laps from "$lib/components/Laps.svelte";
	import { Alert, Card, Loader } from "@kayord/ui";
	import RaceSummary from "./RaceSummary.svelte";
	import TrackSummary from "./TrackSummary.svelte";
	import StopWatch from "$lib/components/StopWatch.svelte";
	import type { DoneEvent, SaveEvent } from "$lib/types";
	import Track from "./Track.svelte";
	import { hub } from "$lib/stores/hub.svelte";

	const query = createRaceSummary(Number(page.params.Id));

	let tracks: Array<Track | undefined> = new Array(4);

	const doneEvent = (evt: DoneEvent) => {
		tracks[evt.trackId]?.doneEvent(evt);
	};

	$effect(() => {
		if (hub.state == "Connected") {
			hub.on("DoneEvent", doneEvent);
			return () => {
				hub.off("DoneEvent", doneEvent);
			};
		}
	});

	const receiveEvent = (evt: SaveEvent) => {
		tracks[evt.trackId]?.receiveEvent(evt);
	};

	$effect(() => {
		if (hub.state == "Connected") {
			hub.on("ReceiveEvent", receiveEvent);
			return () => {
				hub.off("ReceiveEvent", receiveEvent);
			};
		}
	});
</script>

<div class="flex w-full flex-col">
	{#if $query.error}
		<Alert.Root>
			<Alert.Title>An Error Occurred!</Alert.Title>
			<Alert.Description>{$query.error}</Alert.Description>
		</Alert.Root>
	{/if}

	{#if $query.isPending}
		<Loader />
	{/if}

	{#if $query.data}
		<RaceSummary data={$query.data} />
		<div class="flex w-full flex-row gap-2 p-2">
			{#each $query.data?.tracks ?? [] as track}
				<Track {track} refetch={$query.refetch} bind:this={tracks[track.trackId]} />
			{/each}
		</div>
	{/if}
</div>
