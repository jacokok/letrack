import type { PageLoad } from "./$types";

interface Response {
	last10Laps: Array<Lap>;
	fastestLap: Lap;
	totalLaps: number;
	laps: Array<Lap>;
}

interface Lap {
	id: string;
	lastLapId: string;
	trackId: string;
	timestamp: string;
	lapTime: string;
	isFlagged: boolean;
}

export const load = (async ({ fetch, depends }) => {
	depends("track:1");
	const response = await fetch("http://localhost:5021/track/summary/1");
	const data = (await response.json()) as Response;
	return {
		data
	};
}) satisfies PageLoad;
