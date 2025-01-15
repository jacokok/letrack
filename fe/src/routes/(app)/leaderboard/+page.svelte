<script lang="ts">
	import { createScoreboard } from "$lib/api";
	import { Badge, Button, Card, Table } from "@kayord/ui";
	import UsersIcon from "lucide-svelte/icons/users";
	import UserIcon from "lucide-svelte/icons/user";
	import RefreshIcon from "lucide-svelte/icons/refresh-cw";
	import Header from "$lib/components/Header.svelte";
	const query = createScoreboard();
</script>

{#snippet refreshSnippet()}
	<Button size="icon" onclick={$query.refetch} variant="ghost">
		<RefreshIcon />
	</Button>
{/snippet}

<Header right={refreshSnippet} />
<div class="m-2">
	<div class="flex justify-between gap-4">
		<div class="flex w-full flex-col gap-2">
			<div class="mb-2 flex items-center justify-start gap-2">
				<h1 class="font-bold">Dream Team</h1>
				<UsersIcon />
			</div>
			{#each $query.data?.teamSummary ?? [] as team, i}
				{@const position = i + 1}
				<Card.Root class="bg-background flex items-center justify-between gap-2 p-2">
					<div class="flex items-center gap-2">
						<div
							class="bg-muted text-muted-foreground flex h-8 w-8 items-center justify-center rounded-sm text-xl font-bold"
						>
							{position}
						</div>
						<div class="text-xl">{team.name}</div>
					</div>
					<Badge>{team.laps}</Badge>
				</Card.Root>
			{/each}
		</div>
		<div class="flex w-full flex-col gap-2">
			<div class="mb-2 flex items-center justify-start gap-2">
				<h1 class="font-bold">Dream Racer</h1>
				<UserIcon />
			</div>
			{#each $query.data?.playerSummary ?? [] as player, i}
				{@const position = i + 1}
				<Card.Root class="bg-mu flex items-center justify-between gap-2 p-2">
					<div class="flex items-center gap-2">
						<div
							class="bg-muted text-muted-foreground flex h-8 w-8 items-center justify-center rounded-sm text-xl font-bold"
						>
							{position}
						</div>
						<div class="text-xl">{player.name}</div>
					</div>
					<Badge>{player.laps}</Badge>
				</Card.Root>
			{/each}
		</div>
	</div>
</div>
