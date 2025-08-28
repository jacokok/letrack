<script lang="ts">
	import { page } from "$app/state";
	import { createRaceSummary, type RaceSummaryTrack } from "$lib/api";
	import { Button } from "@kayord/ui";
	import Header from "$lib/components/Header.svelte";
	import LineChart from "./LineChart.svelte";
	import { FlagIcon } from "@lucide/svelte";
	import Laps from "./Laps.svelte";

	const query = createRaceSummary(Number(page.params.Id));

	const track1: RaceSummaryTrack | undefined = $derived(
		($query.data?.tracks?.length ?? 0) > 1 && ($query.data?.tracks.length ?? 0) > 1
			? $query.data?.tracks[0]
			: undefined
	);
	const track2: RaceSummaryTrack | undefined = $derived(
		($query.data?.tracks?.length ?? 0) && ($query.data?.tracks.length ?? 0) > 1
			? $query.data?.tracks[1]
			: undefined
	);
</script>

{#snippet right()}
	<div class="flex items-center gap-2">
		<Button variant="ghost" href="/race/{page.params.Id}"><FlagIcon /> Race</Button>
	</div>
{/snippet}

<Header {right} />

<div class="flex m-2">
	<LineChart data={$query.data?.tracks ?? []} />
</div>

<div class="flex items-center gap-2 m-2">
	<Laps data={track1} />
	<Laps data={track2} />
</div>
