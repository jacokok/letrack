<script lang="ts">
	interface Props {
		isOn: boolean;
	}

	let { isOn }: Props = $props();
</script>

<div class="flex flex-col gap-4 rounded-b-2xl bg-black/50 p-4">
	<div
		class={`size-20 rounded-full ${isOn ? "bg-red-500 animate-in fade-in " : "bg-gray-700"}`}
	></div>
	<div
		class={`size-20 rounded-full ${isOn ? "bg-red-500 animate-in fade-in" : "bg-gray-700"}`}
	></div>
</div>
