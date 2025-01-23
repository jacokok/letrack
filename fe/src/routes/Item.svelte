<script lang="ts">
	import { Button, Card } from "@kayord/ui";
	import type { Snippet } from "svelte";

	interface Props {
		title: string;
		description: string;
		href: string;
		icon?: Snippet;
	}
	let { title, description, href, icon }: Props = $props();
</script>

<Card.Root class="w-64">
	<Card.Header class="flex flex-col items-center justify-center">
		{@render icon?.()}
		<Card.Title>{title}</Card.Title>
		<Card.Description>{description}</Card.Description>
	</Card.Header>
	<Card.Footer class="mt-2">
		<Button {href} class="w-full">{title}</Button>
	</Card.Footer>
</Card.Root>
