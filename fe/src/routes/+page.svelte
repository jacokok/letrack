<script lang="ts">
	import Item from "./Item.svelte";
	import RaceIcon from "lucide-svelte/icons/circle-off";
	import ClipboardIcon from "lucide-svelte/icons/clipboard";
	import UserIcon from "lucide-svelte/icons/user";
	import UsersIcon from "lucide-svelte/icons/users";
	import AdminIcon from "lucide-svelte/icons/shield-ellipsis";
	import PracticeIcon from "lucide-svelte/icons/flag-triangle-right";
</script>

<div class="my-8 flex justify-center gap-4">
	<img src="/favicon.svg" alt="LeTrack" class="h-12" />
	<h1 class="text-5xl tracking-tighter">
		<span class="text-secondary">Le</span><span class="text-primary font-bold">Track</span>
	</h1>
</div>

<div class="m-2 flex w-full flex-row flex-wrap justify-center gap-4">
	<Item title="Practice" description="Go to driving range" href="/practice">
		{#snippet icon()}
			<PracticeIcon />
		{/snippet}
	</Item>
	<Item title="Races" description="Show all races or create new race" href="/race">
		{#snippet icon()}
			<RaceIcon />
		{/snippet}
	</Item>
	<Item title="Leaderboard" description="Who is winning" href="/leaderboard">
		{#snippet icon()}
			<ClipboardIcon />
		{/snippet}
	</Item>
</div>

<div class="m-2 mt-8 flex w-full flex-row flex-wrap justify-center gap-4">
	<Item title="Players" description="Manage players/racers" href="/player">
		{#snippet icon()}
			<UserIcon />
		{/snippet}
	</Item>
	<Item title="Team" description="Manage team" href="/team">
		{#snippet icon()}
			<UsersIcon />
		{/snippet}
	</Item>
	<Item title="Admin" description="Advanced Settings" href="/admin">
		{#snippet icon()}
			<AdminIcon />
		{/snippet}
	</Item>
</div>
