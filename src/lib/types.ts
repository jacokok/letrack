interface SaveEvent {
	id: string;
	timestamp: string;
	trackId: string;
}

interface DoneEvent {
	id: string;
	trackId: string;
}

interface TrackSummary {
	last10Laps: Array<Lap>;
	fastestLap?: Lap;
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

export type { SaveEvent, DoneEvent, TrackSummary };
