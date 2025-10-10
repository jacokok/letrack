<script lang="ts">
	import { page } from "$app/state";
	import { createRaceStart, createRaceStop, createRaceSummary } from "$lib/api";
	import { Alert, Button, Loader, Sheet } from "@kayord/ui";
	import RaceSummary from "./RaceSummary.svelte";
	import type { DoneEvent, SaveEvent } from "$lib/types";
	import Track from "./Track.svelte";
	import { hub } from "$lib/stores/hub.svelte";
	import PlayIcon from "@lucide/svelte/icons/play";
	import StopIcon from "@lucide/svelte/icons/square";
	import Header from "$lib/components/Header.svelte";
	import StartRace from "./StartRace.svelte";
	import Lights from "$lib/components/Light/Lights.svelte";
	import { ChartNoAxesCombinedIcon, FlagIcon } from "@lucide/svelte";
	import { Timer } from "$lib/stores/timer.svelte";

	const query = createRaceSummary(Number(page.params.Id));

	let timers: Record<number, Timer> = $state({
		1: new Timer(),
		2: new Timer(),
		3: new Timer(),
		4: new Timer()
	});

	let startRaceOpen = $state(false);
	let startRaceIsActive = $state(false);
	let startRaceSheetOpen = $state(false);
	let startState: { duration: number; laps: number; showCountdown: boolean };

	$effect(() => {
		if (hub.state == "Connected") {
			hub.on("DoneEvent", doneEvent);
			return () => {
				hub.off("DoneEvent", doneEvent);
			};
		}
	});

	$effect(() => {
		if (hub.state == "Connected") {
			hub.on("RaceComplete", raceComplete);
			return () => {
				hub.off("RaceComplete", raceComplete);
			};
		}
	});

	$effect(() => {
		if (hub.state == "Connected") {
			hub.on("ReceiveEvent", receiveEvent);
			return () => {
				hub.off("ReceiveEvent", receiveEvent);
			};
		}
	});

	const doneEvent = (evt: DoneEvent) => {
		query.refetch();
	};

	const raceComplete = (raceId: number) => {
		stopEvent();
		query.refetch();
	};

	const receiveEvent = (evt: SaveEvent) => {
		if (query.data?.race.isActive) {
			timers[evt.trackId]?.start();
		}
	};

	const stopEvent = () => {
		for (const track of query.data?.tracks ?? []) {
			timers[track.trackId]?.stop();
		}
	};

	const stopMutation = createRaceStop();
	const startMutation = createRaceStart();

	const stopStart = async () => {
		if (query.data?.race.isActive) {
			await stopMutation.mutateAsync({ data: { id: Number(page.params.Id) } });
			stopEvent();
			query.refetch();
		} else {
			startRaceOpen = true;
		}
	};

	const startTimers = () => {
		for (const track of query.data?.tracks ?? []) {
			timers[track.trackId].start();
		}
	};

	const startRaceLogic = async () => {
		await startMutation.mutateAsync({
			data: { id: Number(page.params.Id), duration: startState.duration, laps: startState.laps }
		});
		startTimers();
		query.refetch();
	};

	const startRace = async (data: { duration: number; laps: number; showCountdown: boolean }) => {
		startState = data;
		if (!data.showCountdown) {
			startRaceLogic();
		} else {
			startRaceSheetOpen = true;
			setTimeout(() => {
				startRaceIsActive = true;
			}, 1500);
		}
		startRaceOpen = false;
	};

	const onStartRaceComplete = () => {
		startRaceLogic();
	};
	const onStartRacePostComplete = () => {
		startRaceSheetOpen = false;
	};
	const onStartRacePreComplete = () => {
		startRaceLogic();
	};

	const hideOptions = $derived(query.data?.race.timeRemaining != undefined);
</script>

{#snippet right()}
	{#if query.data}
		<div class="flex items-center gap-2">
			<Button variant="ghost" href="/race/{page.params.Id}/stats">
				<ChartNoAxesCombinedIcon /> Stats
			</Button>

			{#if !query.data?.race.isActive}
				<Button variant="secondary" href="/race/post/{page.params.Id}"
					><FlagIcon /> Post Race</Button
				>
			{/if}
			<Button size="icon" onclick={stopStart}>
				{#if query.data?.race.isActive}
					<StopIcon />
				{:else}
					<PlayIcon />
				{/if}
			</Button>
		</div>
	{/if}
{/snippet}

<Header {right} />

<Sheet.Root bind:open={startRaceSheetOpen}>
	<Sheet.Content
		side="top"
		interactOutsideBehavior="ignore"
		escapeKeydownBehavior="ignore"
		class="px-0 [&>button]:hidden"
	>
		<div>
			<Lights
				isActive={startRaceIsActive}
				onComplete={onStartRaceComplete}
				onPreComplete={onStartRacePreComplete}
				onPostComplete={onStartRacePostComplete}
			/>
		</div>
	</Sheet.Content>
</Sheet.Root>

<div class="flex w-full flex-col">
	<StartRace bind:open={startRaceOpen} cb={startRace} {hideOptions} />
	{#if query.error}
		<Alert.Root>
			<Alert.Title>An Error Occurred!</Alert.Title>
			<Alert.Description>{query.error}</Alert.Description>
		</Alert.Root>
	{/if}

	{#if query.isPending}
		<Loader />
	{/if}

	{#if query.data}
		<RaceSummary data={query.data} />
		<div class="flex w-full flex-row gap-2 p-2">
			{#each query.data?.tracks ?? [] as track}
				<Track {track} timer={timers[track.trackId]} />
			{/each}
		</div>
	{/if}
</div>
