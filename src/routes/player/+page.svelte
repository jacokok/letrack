<script lang="ts">
	import { createTrackSummary } from "$lib/api";
	import { Button } from "@kayord/ui";
	import SettingsIcon from "lucide-svelte/icons/settings";

	const query = createTrackSummary(1);
</script>

<Button class="fixed right-4 top-4 z-50" size="icon" variant="secondary"><SettingsIcon /></Button>

<div class="bg-secondary text-secondary-foreground">Test</div>
<div class="bg-accent text-accent-foreground">Test</div>
<pre>{JSON.stringify($query.data)}</pre>
