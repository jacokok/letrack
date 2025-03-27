<script lang="ts">
	import { createLeaderboard } from "$lib/api";
	import { Badge, Button, Card, Table } from "@kayord/ui";
	import UsersIcon from "@lucide/svelte/icons/users";
	import UserIcon from "@lucide/svelte/icons/user";
	import FileIcon from "@lucide/svelte/icons/file";
	import RefreshIcon from "@lucide/svelte/icons/refresh-cw";
	import Header from "$lib/components/Header.svelte";
	import { PUBLIC_API_URL } from "$env/static/public";
	import PlayerAvatar from "$lib/components/PlayerAvatar.svelte";
	const query = createLeaderboard();

	const exportToCSV = () => {
		window.location.assign(`${PUBLIC_API_URL}/export`);
	};
</script>

{#snippet refreshSnippet()}
	<div>
		<Button size="icon" onclick={exportToCSV} variant="ghost">
			<FileIcon />
		</Button>
		<Button size="icon" onclick={$query.refetch} variant="ghost">
			<RefreshIcon />
		</Button>
	</div>
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
						<PlayerAvatar name={player.name} isSmall />
						<div class="flex flex-col">
							<div class="leading-4">
								{player.name}
							</div>
							<div class="text-muted-foreground text-xs leading-3">
								{player.nickName ? ` ${player.nickName}` : ""}
							</div>
						</div>
					</div>
					<Badge>{player.laps}</Badge>
				</Card.Root>
			{/each}
		</div>
	</div>
</div>
