<script lang="ts">
	import { PUBLIC_API_URL } from "$env/static/public";
	import { Badge, Card, Label, ProgressLoading, Switch } from "@kayord/ui";
	import {
		CircleAlertIcon,
		CircleCheckIcon,
		CircleQuestionMarkIcon,
		CircleXIcon
	} from "@lucide/svelte";
	import { onMount } from "svelte";

	interface HealthResponse {
		status: HealthStatus;
		totalDuration: string;
		entries: HealthCheckEntries;
	}

	interface HealthCheckEntries {
		[key: string]: HealthCheckEntry;
	}

	interface HealthCheckEntry {
		description?: string;
		duration: string;
		exception?: string;
		status: HealthStatus;
	}

	type HealthStatus = "Healthy" | "Unhealthy" | "Degraded" | "Unknown";

	let isLoading = $state(false);
	let healthData: HealthResponse = $state({ status: "Unknown", totalDuration: "0", entries: {} });

	let showJSON = $state(false);

	onMount(async () => {
		healthData = await getHealth();
	});

	const getHealth = async (): Promise<HealthResponse> => {
		try {
			isLoading = true;
			const res = await fetch(`${PUBLIC_API_URL}/health`);
			const results = (await res.json()) as HealthResponse;
			return results;
		} catch (error) {
			console.error("Error fetching health data:", error);
			return { status: "Unknown", totalDuration: "0", entries: {} };
		} finally {
			isLoading = false;
		}
	};
</script>

<div class="m-2">
	<div class="flex justify-between mb-2">
		<h2 class="text-2xl">Health</h2>
		<div class="flex items-center gap-2">
			<Label for="show-json">JSON</Label>
			<Switch id="show-json" bind:checked={showJSON} />
		</div>
	</div>
	{#if isLoading}
		<ProgressLoading class="h-2" />
	{/if}

	{#if showJSON}
		<div class="mt-4">
			<pre>
{JSON.stringify(healthData, null, 2)}
	</pre>
		</div>
	{/if}

	{#if !showJSON}
		<div class="flex flex-col gap-4">
			{#each Object.keys(healthData.entries) as key (key)}
				<Card.Root class="flex flex-col justify-center items-center gap-1">
					{@render statusIcon(healthData.entries[key].status)}
					<Card.Title>{key}</Card.Title>
					<Card.Description>{healthData.entries[key].description}</Card.Description>
					{@render statusBadge(healthData.entries[key].status)}
				</Card.Root>
			{/each}
		</div>
	{/if}
</div>

{#snippet statusBadge(type: HealthStatus)}
	{#if type === "Healthy"}
		<Badge class="bg-primary text-primary-foreground">{type}</Badge>
	{:else if type === "Degraded"}
		<Badge class="bg-secondary text-secondary-foreground">{type}</Badge>
	{:else if type === "Unhealthy"}
		<Badge class="bg-destructive text-destructive-foreground">{type}</Badge>
	{:else}
		<Badge class="bg-muted text-muted-foreground">{type}</Badge>
	{/if}
{/snippet}

{#snippet statusIcon(type: HealthStatus)}
	{#if type === "Healthy"}
		<div
			class="h-10 w-10 bg-primary text-primary-foreground flex items-center justify-center rounded-full mb-2"
		>
			<CircleCheckIcon />
		</div>
	{:else if type === "Degraded"}
		<div
			class="h-10 w-10 bg-secondary text-secondary-foreground flex items-center justify-center rounded-full mb-2"
		>
			<CircleAlertIcon />
		</div>
	{:else if type === "Unhealthy"}
		<div
			class="h-10 w-10 bg-destructive text-destructive-foreground flex items-center justify-center rounded-full mb-2"
		>
			<CircleXIcon />
		</div>
	{:else}
		<div
			class="h-10 w-10 bg-muted text-muted-foreground flex items-center justify-center rounded-full mb-2"
		>
			<CircleQuestionMarkIcon />
		</div>
	{/if}
{/snippet}
