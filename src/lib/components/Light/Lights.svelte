<script lang="ts">
	import Light from "./Light.svelte";
	import { tick } from "svelte";
	import { Confetti } from "svelte-confetti";

	interface Props {
		isActive?: boolean;
		onComplete?: () => void;
		onPreComplete?: () => void;
		onPostComplete?: () => void;
	}

	let { isActive = $bindable(false), onComplete, onPreComplete, onPostComplete }: Props = $props();

	let lights = $state([false, false, false, false, false]);

	const startConfetti = async () => {
		showConfetti = false;
		await tick();
		showConfetti = true;
	};

	let showConfetti = $state(false);

	$effect(() => {
		if (isActive) {
			let curLight = 0;
			const turnOnNextLight = () => {
				lights[curLight] = true;
			};

			const turnOffAllLights = () => {
				lights = [false, false, false, false, false];
			};

			const go = () => {
				// Random value between 300 and 3000
				const randomInterval = Math.floor(Math.random() * 2700) + 300;
				setTimeout(() => {
					onPreComplete?.();
				}, randomInterval - 100);
				clearInterval(timer);
				setTimeout(() => {
					turnOffAllLights();
					startConfetti();
					isActive = false;
					onComplete?.();
				}, randomInterval);
				setTimeout(() => {
					onPostComplete?.();
				}, randomInterval + 400);
			};

			turnOffAllLights();

			let timer = setInterval(() => {
				if (curLight <= 4) {
					turnOnNextLight();
				} else if (curLight >= 5) {
					go();
				}
				curLight++;
			}, 1000);

			return () => {
				clearInterval(timer);
			};
		} else {
			lights = [false, false, false, false, false];
		}
	});
</script>

<div class="flex w-full flex-col items-center">
	<div class="flex w-full items-center">
		<div class="h-4 w-full bg-black/70"></div>
		<div class="flex h-52 min-w-[680px] items-center justify-center rounded-xl bg-black/70">
			<img src="/favicon.svg" alt="LeTrack" class="h-32" />
			{#if showConfetti}
				<Confetti y={[0.25, 0.5]} x={[-4, 4]} />
			{/if}
			<h1 class="ml-2 text-7xl tracking-tighter">
				<span class="text-secondary">Le</span><span class="font-bold text-primary">Track</span>
			</h1>
		</div>
		<div class="h-4 w-full bg-black/70"></div>
	</div>

	<div class="flex w-fit items-center gap-4 rounded-md">
		{#each lights as light}
			<Light isOn={light} />
		{/each}
	</div>
</div>
