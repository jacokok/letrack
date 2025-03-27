<script lang="ts">
	import { Avatar } from "@kayord/ui";

	interface Props {
		name: string;
		url?: string;
		isSmall?: boolean;
	}

	let { name, url, isSmall = false }: Props = $props();

	const getInitials = (name: string) => {
		const initials = name.match(/\b\w/g) || [];
		return `${initials.shift() || ""}${initials.pop() || ""}`.toUpperCase();
	};

	const getRandomColorAndTextColor = (text: string) => {
		if (text.length === 0) {
			return { backgroundColor: "#FFFFFF", textColor: "#000000" }; // Default for empty text
		}

		// Generate a pseudo-random number based on the input text.
		let hash = 0;
		for (let i = 0; i < text.length; i++) {
			hash = text.charCodeAt(i) + ((hash << 5) - hash);
		}

		// Convert the hash to an RGB color.
		const r = Math.abs((hash & 0xff0000) >> 16) % 256;
		const g = Math.abs((hash & 0x00ff00) >> 8) % 256;
		const b = Math.abs(hash & 0x0000ff) % 256;

		const backgroundColor = `#${r.toString(16).padStart(2, "0")}${g.toString(16).padStart(2, "0")}${b.toString(16).padStart(2, "0")}`;

		// Calculate luminance to determine appropriate text color.
		const luminance = (0.299 * r + 0.587 * g + 0.114 * b) / 255;
		const textColor = luminance > 0.5 ? "#000000" : "#FFFFFF";

		return { backgroundColor, textColor };
	};

	const color = $derived(getRandomColorAndTextColor(name));
	const sizeClass = $derived(isSmall ? "text-xs size-8" : "");
</script>

<Avatar.Root class={sizeClass}>
	{#if url}
		<Avatar.Image src={url} alt={name} />
	{/if}
	<Avatar.Fallback style={`background: ${color.backgroundColor}; color: ${color.textColor}`}>
		{getInitials(name)}
	</Avatar.Fallback>
</Avatar.Root>
