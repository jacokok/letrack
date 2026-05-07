<script lang="ts">
	import { page } from "$app/state";
	import { createRaceSummary } from "$lib/api";
	import { Badge, Button, Card, Item, Separator, StatusDot } from "@kayord/ui";
	import Header from "$lib/components/Header.svelte";
	import StatsChart from "./StatsChart.svelte";
	import { FlagIcon } from "@lucide/svelte";
	import PlayerAvatar from "$lib/components/PlayerAvatar.svelte";
	import { trackColors } from "$lib/util";

	const query = createRaceSummary(() => Number(page.params.Id));

	const getColor = (trackId: number) => {
		const key = `track${trackId}` as unknown as keyof typeof trackColors;
		return trackColors[key];
	};
</script>

{#snippet right()}
	<div class="flex items-center gap-2">
		<Button variant="ghost" href="/race/{page.params.Id}"><FlagIcon /> Race</Button>
	</div>
{/snippet}

<Header {right} />

<div class="m-2 flex flex-col gap-6">
	{#if query.isPending}
		<div>Loading...</div>
	{:else if query.error}
		<div>Error: {query.error.reason}</div>
	{:else}
		<Card.Root class="w-full">
			<Item.Group>
				{#each query.data?.tracks as track, index (track.trackId)}
					<div class="flex w-full flex-col">
						<Item.Root class="py-0">
							<Item.Media>
								<PlayerAvatar name={track.player.name} />
							</Item.Media>
							<Item.Content>
								<Item.Title>{track.player.nickName || track.player.name}</Item.Title>
								<Item.Description>Track {track.trackId}</Item.Description>
							</Item.Content>
							<Item.Actions class="flex gap-4">
								{@const c = getColor(track.trackId)}
								<Separator orientation="vertical" class="h-10!" style={`background-color: ${c};`} />
								<h3>{track.totalLaps}</h3>
							</Item.Actions>
						</Item.Root>
						{#if index !== (query.data?.tracks?.length ?? 0) - 1}
							<Item.Separator />
						{/if}
					</div>
				{/each}
			</Item.Group>
		</Card.Root>
		<StatsChart data={query.data?.tracks ?? []} />
	{/if}
</div>
