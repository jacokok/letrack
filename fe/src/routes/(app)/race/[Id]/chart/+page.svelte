<script lang="ts">
	import { page } from "$app/state";
	import { createRaceSummary } from "$lib/api";
	import { Alert, Loader, Sheet } from "@kayord/ui";
	import Header from "$lib/components/Header.svelte";
	import Lights from "$lib/components/Light/Lights.svelte";
	import Track from "../Track.svelte";
	import LineChart from "../LineChart.svelte";

	const query = createRaceSummary(Number(page.params.Id));
</script>

<LineChart data={$query.data?.tracks ?? []} />
