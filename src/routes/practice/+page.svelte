<script lang="ts">
	import { Button, Label, Popover, Slider } from "@kayord/ui";
	import SettingsIcon from "lucide-svelte/icons/settings";
	import Track from "./Track.svelte";
	import { practice } from "$lib/stores/practice.svelte";
	import type { DoneEvent, SaveEvent } from "$lib/types";
	import { hub } from "$lib/stores/hub.svelte";
	import Chart from "$lib/components/Chart.svelte";

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

<Popover.Root>
	<Popover.Trigger class="fixed right-4 top-4 z-10">
		<Button size="icon" variant="secondary">
			<SettingsIcon />
		</Button>
	</Popover.Trigger>
	<Popover.Content class="flex flex-col gap-4">
		<Label>Number of tracks</Label>
		<Slider
			bind:value={practice.value.numberOfTracks}
			min={1}
			max={2}
			step={1}
			class="max-w-[100%]"
		/>
	</Popover.Content>
</Popover.Root>

<div class="m-2 flex flex-row gap-2">
	{#each { length: practice.value.numberOfTracks[0] }, index}
		{@const curTrack = index + 1}
		<Track trackId={curTrack} bind:this={tracks[curTrack]} />
	{/each}
</div>

<div>
	<Chart />
</div>
