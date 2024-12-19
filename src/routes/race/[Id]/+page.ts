import type { RaceSummary } from "$lib/types";
import type { PageLoad } from "./$types";

export const load = (async ({ fetch, depends, params }) => {
	depends(`race:${params.Id}`);
	const response = await fetch(`http://localhost:5021/race/summary/${params.Id}`);
	const raceSummary = (await response.json()) as RaceSummary;
	return {
		raceSummary
	};
}) satisfies PageLoad;
