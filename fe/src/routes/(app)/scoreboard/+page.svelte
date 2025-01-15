<script lang="ts">
	import { createScoreboard } from "$lib/api";
	const query = createScoreboard();
</script>

<div class="m-2">
	{JSON.stringify($query.data)}
</div>
