<script lang="ts">
	import { Button, Label, Popover, Switch } from "@kayord/ui";
	import SettingsIcon from "lucide-svelte/icons/settings";
	import Track from "./Track.svelte";
	import { practice } from "$lib/stores/practice.svelte";
	import type { DoneEvent, SaveEvent } from "$lib/types";
	import { hub } from "$lib/stores/hub.svelte";
	import Chart from "$lib/components/Chart.svelte";
	import Header from "$lib/components/Header.svelte";

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

{#snippet right()}
	<Popover.Root>
		<Popover.Trigger class="">
			<Button size="icon" variant="ghost">
				<SettingsIcon />
			</Button>
		</Popover.Trigger>
		<Popover.Content class="flex w-40 flex-col gap-4">
			<div class="flex items-center justify-start gap-2">
				<Label for="track1">Track 1</Label>
				<Switch id="track1" bind:checked={practice.value.track1} />
			</div>
			<div class="flex items-center justify-start gap-2">
				<Label for="track2">Track 2</Label>
				<Switch id="track2" bind:checked={practice.value.track2} />
			</div>
			<div class="flex items-center justify-start gap-2">
				<Label for="track3">Track 3</Label>
				<Switch id="track3" bind:checked={practice.value.track3} />
			</div>
			<div class="flex items-center justify-start gap-2">
				<Label for="track4">Track 4</Label>
				<Switch id="track4" bind:checked={practice.value.track4} />
			</div>
		</Popover.Content>
	</Popover.Root>
{/snippet}

<Header {right} />

<div class="m-2 flex flex-row gap-2">
	{#if practice.value.track1 == true}
		<Track trackId={1} bind:this={tracks[1]} />
	{/if}
	{#if practice.value.track2 == true}
		<Track trackId={2} bind:this={tracks[2]} />
	{/if}
	{#if practice.value.track3 == true}
		<Track trackId={3} bind:this={tracks[3]} />
	{/if}
	{#if practice.value.track4 == true}
		<Track trackId={4} bind:this={tracks[4]} />
	{/if}
</div>

<div>
	<Chart />
</div>
