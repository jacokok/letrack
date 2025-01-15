<script lang="ts">
	import { Button, Card } from "@kayord/ui";
</script>

{#snippet item(title: string, description: string, href: string)}
	<Card.Root class="w-64">
		<Card.Header class="flex flex-col items-center justify-center">
			<Card.Title>{title}</Card.Title>
			<Card.Description>{description}</Card.Description>
		</Card.Header>
		<Card.Footer class="mt-2">
			<Button {href} class="w-full">{title}</Button>
		</Card.Footer>
	</Card.Root>
{/snippet}

{#snippet section()}
	<div>Race</div>
{/snippet}

<div class="my-8 flex justify-center gap-4">
	<img src="/favicon.svg" alt="LeTrack" class="h-12" />
	<h1 class="text-5xl tracking-tighter">
		<span class="text-secondary">Le</span><span class="text-primary font-bold">Track</span>
	</h1>
</div>

<div class="m-2 flex w-full flex-row flex-wrap justify-center gap-4">
	{@render item("Practice", "Go to driving range", "/practice")}
	{@render item("Races", "Show all races or create new race", "/race")}
	{@render item("Leaderboard", "Who is winning", "/leaderboard")}
	{@render item("Players", "Manage players/racers", "/player")}
	{@render item("Team", "Manage team", "/team")}
</div>
