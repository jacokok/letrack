import { defineConfig } from "orval";

export default defineConfig({
	iot: {
		input: "./swagger.json",
		output: {
			mode: "tags",
			workspace: "./src/lib/api/generated",
			target: "api.ts",
			client: "svelte-query",
			headers: false,
			clean: true,
			override: {
				fetch: {
					includeHttpResponseReturnType: false
				},
				mutator: {
					path: "../mutator/customInstance.svelte.ts",
					name: "customInstance"
				},
				query: {
					signal: false
				}
			}
		}
	}
});
