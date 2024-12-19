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
	race?: Race;
}

interface RaceSummary {
	laps: Array<Lap>;
	fastestLap?: Lap;
	totalLaps: number;
	race: Race;
}

interface Race {
	id: number;
	name: string;
	isActive: boolean;
	raceTracks: Array<RaceTrack>;
}

interface RaceTrack {
	raceId: number;
	trackId: number;
	playerId: number;
	player: Player;
}

interface Player {
	id: number;
	name: string;
	nickName?: string;
}

interface Lap {
	id: string;
	lastLapId: string;
	trackId: string;
	timestamp: string;
	lapTime?: string;
	lapTimeDifference?: string;
	isFlagged: boolean;
	lapNumber: number;
}

export type { SaveEvent, DoneEvent, TrackSummary, Lap, RaceSummary };
