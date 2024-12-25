<script lang="ts">
	interface Props {
		isOn: boolean;
	}

	let { isOn }: Props = $props();
</script>

<div class={`size-10 rounded-full ${isOn ? "bg-red-500" : "bg-black"} border-4 border-muted`}></div>
