import type { Config } from "tailwindcss";
import { kayordPlugin } from "@kayord/tw-plugin";
import { fontFamily } from "tailwindcss/defaultTheme";
import tailwindcssAnimate from "tailwindcss-animate";

export default {
	darkMode: ["class"],
	content: [
		"./src/**/*.{html,js,svelte,ts}",
		"./node_modules/@kayord/ui/**/*.{html,js,svelte,ts}",
		"./node_modules/layerchart/**/*.{svelte,js}"
	],
	theme: {
		extend: {
			colors: {
				surface: {
					content: "hsl(var(--card-foreground))",
					100: "hsl(var(--background))",
					200: "hsl(var(---muted))",
					300: "hsl(var(--background))"
				}
			},
			fontFamily: {
				sans: [...fontFamily.sans]
			}
		}
	},
	plugins: [kayordPlugin, tailwindcssAnimate]
} satisfies Config;
