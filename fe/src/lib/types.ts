import type { DTOLapDTO } from "./api";

interface SaveEvent {
	id: string;
	timestamp: string;
	trackId: number;
}

interface DoneEvent {
	id: string;
	trackId: number;
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

type Lap = DTOLapDTO;
// interface Lap {
// 	id: string;
// 	lastLapId: string;
// 	trackId: string;
// 	timestamp: string;
// 	lapTime?: string;
// 	lapTimeDifference?: string;
// 	isFlagged: boolean;
// 	lapNumber: number;
// }

interface ValidationError {
	statusCode: string;
	message: string;
	errors?: ValidationErrorItem;
}

interface ValidationErrorItem {
	[key: string]: string[];
}

// eslint-disable-next-line @typescript-eslint/no-explicit-any
const isValidationError = (apiError: any): apiError is ValidationError => {
	return "statusCode" in apiError && "message" in apiError && "errors" in apiError;
};

type ErrorResponseErrors = { [key: string]: string[] };

interface ErrorResponse {
	errors: ErrorResponseErrors;
	message: string;
	statusCode: number;
}

interface InternalErrorResponse {
	code: number;
	note: string;
	reason: string;
	status: string;
}

type ApiError = ErrorResponse | InternalErrorResponse | Error | unknown | void;

interface ErrorGenericErrors {
	[key: string]: string[];
}

interface ErrorGeneric {
	statusCode: number;
	message: string;
	summary: string;
	errors: ErrorGenericErrors;
}

// eslint-disable-next-line @typescript-eslint/no-explicit-any
const isInternalErrorResponse = (error: any): error is InternalErrorResponse => {
	return "status" in error && "code" in error && "reason" in error;
};

const isErrorResponse = (
	// eslint-disable-next-line @typescript-eslint/no-explicit-any
	error: any
): error is ErrorResponse => {
	return "statusCode" in error && "message" in error && "errors" in error;
};

// eslint-disable-next-line @typescript-eslint/no-explicit-any
function isError(err: any | unknown): err is Error {
	return (err as Error).message !== undefined;
}

const getError = (inputError: ApiError): ErrorGeneric => {
	const error: ErrorGeneric = {
		statusCode: 0,
		message: "",
		summary: "",
		errors: {}
	};

	if (inputError == null) {
		return error;
	}

	if (isErrorResponse(inputError)) {
		error.message = inputError.message;
		error.statusCode = inputError.statusCode;
		error.errors = inputError.errors;
		error.summary = Object.values(inputError.errors)
			.map((err) => {
				return err.join();
			})
			.join();
	} else if (isInternalErrorResponse(inputError)) {
		error.message = inputError.reason;
		error.statusCode = inputError.code;
		error.summary = inputError.note;
		error.errors = {
			code: [inputError.code.toString()],
			status: [inputError.status],
			reason: [inputError.reason],
			note: [inputError.note]
		};
	} else if (isError(inputError)) {
		error.message = inputError.message;
		error.statusCode = 0;
		error.summary = inputError.cause as string;
		error.errors = {
			name: [inputError.name],
			cause: [inputError.cause as string],
			stack: [inputError.stack as string]
		};
	} else {
		error.message = "An unknown error occurred";
	}
	return error;
};

export type { SaveEvent, DoneEvent, TrackSummary, Lap, RaceSummary, ValidationError };
export { getError, isValidationError };
