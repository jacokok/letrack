<script lang="ts">
	import { AlertDialog, Button, Card, toast } from "@kayord/ui";
	import TrashIcon from "@lucide/svelte/icons/trash-2";
	import LapIcon from "@lucide/svelte/icons/timer-reset";
	import InvalidLapIcon from "@lucide/svelte/icons/timer-off";
	import EventIcon from "@lucide/svelte/icons/party-popper";
	import RaceIcon from "@lucide/svelte/icons/circle-off";
	import {
		createAdminClearEvents,
		createAdminClearAll,
		createAdminClearLaps,
		createAdminClearRaces,
		createAdminClearInvalidLaps,
		createAdminSnapshot
	} from "$lib/api";
	import { ArchiveIcon } from "@lucide/svelte";

	const clearEventMutation = createAdminClearEvents();
	const clearInvalidLapsMutation = createAdminClearInvalidLaps();
	const clearRacesMutation = createAdminClearRaces();
	const clearLapsMutation = createAdminClearLaps();
	const clearAllMutation = createAdminClearAll();
	const snapshot = createAdminSnapshot();

	let isOpen = $state(false);
	let eventType = $state<"event" | "race" | "snapshot" | "lap-invalid" | "lap" | "all" | "none">(
		"none"
	);

	const action = async () => {
		isOpen = false;
		if (eventType == "none") {
			return;
		} else if (eventType == "snapshot") {
			toast.info("Disconnect active connections from database and creating snapshot.");
			await $snapshot.mutateAsync();
			toast.info("Created Snapshot");
		} else if (eventType == "race") {
			await $clearRacesMutation.mutateAsync();
			toast.info("Deleted all races");
		} else if (eventType == "event") {
			await $clearEventMutation.mutateAsync();
			toast.info("Deleted all events");
		} else if (eventType == "lap-invalid") {
			await $clearInvalidLapsMutation.mutateAsync();
			toast.info("Deleted invalid laps");
		} else if (eventType == "lap") {
			await $clearLapsMutation.mutateAsync();
			toast.info("Deleted all laps");
		} else if (eventType == "all") {
			await $clearAllMutation.mutateAsync();
			toast.info("Deleted all data");
		}
	};
</script>

<div class="m-2">
	<Card.Root>
		<Card.Header>
			<Card.Title>Admin</Card.Title>
			<Card.Description>Advanced option for admin which is very dangerous</Card.Description>
		</Card.Header>
		<Card.Content class="flex flex-col gap-4">
			<Button
				class="w-full"
				variant="secondary"
				onclick={() => {
					isOpen = true;
					eventType = "snapshot";
				}}
			>
				<ArchiveIcon />Create Snapshot
			</Button>
			<Button
				class="w-full"
				onclick={() => {
					isOpen = true;
					eventType = "event";
				}}
			>
				<EventIcon />Clear Events
			</Button>
			<Button
				class="w-full"
				onclick={() => {
					isOpen = true;
					eventType = "lap-invalid";
				}}
			>
				<InvalidLapIcon />Clear Invalid Laps
			</Button>
			<Button
				class="w-full"
				variant="destructive"
				onclick={() => {
					isOpen = true;
					eventType = "lap";
				}}
			>
				<LapIcon />Clear All Laps
			</Button>
			<Button
				class="w-full"
				variant="destructive"
				onclick={() => {
					isOpen = true;
					eventType = "race";
				}}
			>
				<RaceIcon />Clear All Races
			</Button>
			<Button
				class="w-full"
				variant="destructive"
				onclick={() => {
					isOpen = true;
					eventType = "all";
				}}
			>
				<TrashIcon />Clear All
			</Button>
		</Card.Content>
	</Card.Root>
</div>

<AlertDialog.Root bind:open={isOpen}>
	<AlertDialog.Content>
		<AlertDialog.Header>
			<AlertDialog.Title>Are you absolutely sure?</AlertDialog.Title>
			<AlertDialog.Description>
				You are about to run a destructive action that will completely erase your data.
			</AlertDialog.Description>
		</AlertDialog.Header>
		<AlertDialog.Footer>
			<AlertDialog.Cancel>Cancel</AlertDialog.Cancel>
			<AlertDialog.Action class="bg-destructive text-destructive-foreground" onclick={action}>
				Delete
			</AlertDialog.Action>
		</AlertDialog.Footer>
	</AlertDialog.Content>
</AlertDialog.Root>
