<script lang="ts">
	interface Props {
		isOn: boolean;
	}

	let { isOn }: Props = $props();
</script>

<div class="border-x-muted border-b-muted flex flex-col gap-4 rounded-b-2xl border-2 bg-black p-4">
	<div
		class={`size-20 rounded-full ${isOn ? "animate-in fade-in bg-red-500" : "bg-[#0c0d12]"} border-muted border-2`}
	></div>
	<div
		class={`size-20 rounded-full ${isOn ? "animate-in fade-in bg-red-500" : "bg-[#0c0d12]"} border-muted border-2`}
	></div>
</div>
