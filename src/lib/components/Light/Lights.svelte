<script>
	import Light from "./Light.svelte";

	let lights = $state([false, false, false, false, false]);

	export const start = () => {
		let curLight = 0;

		const turnOnNextLight = () => {
			lights[curLight] = true;
		};
		let timerId = setInterval(() => turnOnNextLight(), 1000);
	};
</script>

<div class="flex w-fit items-center gap-2 rounded-md bg-gray-600 p-1">
	{#each lights as light}
		<Light isOn={light} />
	{/each}
</div>
