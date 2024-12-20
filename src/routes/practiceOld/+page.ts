import type { TrackSummary } from "$lib/types";
import type { PageLoad } from "./$types";

export const load = (async ({ fetch, depends }) => {
	depends("track:1");
	const response = await fetch("http://localhost:5021/track/summary/1");
	const trackSummary = (await response.json()) as TrackSummary;
	return {
		trackSummary
	};
}) satisfies PageLoad;
