<script lang="ts">
	import { Card } from "@kayord/ui";
	import TimerIcon from "lucide-svelte/icons/hourglass";

	interface Props {
		endDateTime: string;
	}

	let { endDateTime }: Props = $props();

	let countDown = $state({
		hours: "00",
		minutes: "00",
		seconds: "00"
	});

	$effect(() => {
		const interval = setInterval(() => {
			const now = new Date();
			const endTime = new Date(endDateTime);
			const diff = endTime.getTime() - now.getTime();
			if (diff > 0) {
				const timer = new Date(diff);
				countDown = {
					hours: timer.getUTCHours().toString().padStart(2, "0"),
					minutes: timer.getUTCMinutes().toString().padStart(2, "0"),
					seconds: timer.getUTCSeconds().toString().padStart(2, "0")
				};
			} else {
				countDown = {
					hours: "00",
					minutes: "00",
					seconds: "00"
				};
			}
		}, 1000);
		return () => {
			clearInterval(interval);
		};
	});
</script>

<Card.Root class="flex w-72 items-center gap-2 p-2">
	<TimerIcon class="size-10" />
	<h1 class="flex gap-2 text-5xl font-bold">
		{countDown.hours}:{countDown.minutes}:{countDown.seconds}
	</h1>
</Card.Root>
