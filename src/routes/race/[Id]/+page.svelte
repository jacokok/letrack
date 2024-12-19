<script lang="ts">
	import Laps from "$lib/components/Laps.svelte";
	import type { PageData } from "./$types";

	let { data }: { data: PageData } = $props();
</script>

<div class="flex w-full flex-row">
	<Laps laps={data.raceSummary.laps} fastestLap={data.raceSummary.fastestLap} />
</div>
