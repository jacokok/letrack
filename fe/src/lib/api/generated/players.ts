/**
 * Generated by orval v7.8.0 🍺
 * Do not edit manually.
 * letrack-api
 * OpenAPI spec version: v1
 */
import { createMutation, createQuery } from "@tanstack/svelte-query";
import type {
	CreateMutationOptions,
	CreateMutationResult,
	CreateQueryOptions,
	CreateQueryResult,
	DataTag,
	MutationFunction,
	QueryClient,
	QueryFunction,
	QueryKey
} from "@tanstack/svelte-query";

import type {
	EntitiesPlayer,
	InternalErrorResponse,
	PlayersInsertRequest,
	PlayersUpdateRequest
} from "./api.schemas";

import { customInstance } from "../mutator/customInstance.svelte";
import type { ErrorType, BodyType } from "../mutator/customInstance.svelte";

export const playersUpdate = (playersUpdateRequest: BodyType<PlayersUpdateRequest>) => {
	return customInstance<EntitiesPlayer>({
		url: `/players`,
		method: "PUT",
		headers: { "Content-Type": "application/json" },
		data: playersUpdateRequest
	});
};

export const getPlayersUpdateMutationOptions = <
	TError = ErrorType<InternalErrorResponse>,
	TContext = unknown
>(options?: {
	mutation?: CreateMutationOptions<
		Awaited<ReturnType<typeof playersUpdate>>,
		TError,
		{ data: BodyType<PlayersUpdateRequest> },
		TContext
	>;
}): CreateMutationOptions<
	Awaited<ReturnType<typeof playersUpdate>>,
	TError,
	{ data: BodyType<PlayersUpdateRequest> },
	TContext
> => {
	const mutationKey = ["playersUpdate"];
	const { mutation: mutationOptions } = options
		? options.mutation && "mutationKey" in options.mutation && options.mutation.mutationKey
			? options
			: { ...options, mutation: { ...options.mutation, mutationKey } }
		: { mutation: { mutationKey } };

	const mutationFn: MutationFunction<
		Awaited<ReturnType<typeof playersUpdate>>,
		{ data: BodyType<PlayersUpdateRequest> }
	> = (props) => {
		const { data } = props ?? {};

		return playersUpdate(data);
	};

	return { mutationFn, ...mutationOptions };
};

export type PlayersUpdateMutationResult = NonNullable<Awaited<ReturnType<typeof playersUpdate>>>;
export type PlayersUpdateMutationBody = BodyType<PlayersUpdateRequest>;
export type PlayersUpdateMutationError = ErrorType<InternalErrorResponse>;

export const createPlayersUpdate = <TError = ErrorType<InternalErrorResponse>, TContext = unknown>(
	options?: {
		mutation?: CreateMutationOptions<
			Awaited<ReturnType<typeof playersUpdate>>,
			TError,
			{ data: BodyType<PlayersUpdateRequest> },
			TContext
		>;
	},
	queryClient?: QueryClient
): CreateMutationResult<
	Awaited<ReturnType<typeof playersUpdate>>,
	TError,
	{ data: BodyType<PlayersUpdateRequest> },
	TContext
> => {
	const mutationOptions = getPlayersUpdateMutationOptions(options);

	return createMutation(mutationOptions, queryClient);
};
export const playersList = () => {
	return customInstance<EntitiesPlayer[]>({ url: `/players`, method: "GET" });
};

export const getPlayersListQueryKey = () => {
	return [`/players`] as const;
};

export const getPlayersListQueryOptions = <
	TData = Awaited<ReturnType<typeof playersList>>,
	TError = ErrorType<InternalErrorResponse>
>(options?: {
	query?: Partial<CreateQueryOptions<Awaited<ReturnType<typeof playersList>>, TError, TData>>;
}) => {
	const { query: queryOptions } = options ?? {};

	const queryKey = queryOptions?.queryKey ?? getPlayersListQueryKey();

	const queryFn: QueryFunction<Awaited<ReturnType<typeof playersList>>> = () => playersList();

	return { queryKey, queryFn, ...queryOptions } as CreateQueryOptions<
		Awaited<ReturnType<typeof playersList>>,
		TError,
		TData
	> & { queryKey: DataTag<QueryKey, TData, TError> };
};

export type PlayersListQueryResult = NonNullable<Awaited<ReturnType<typeof playersList>>>;
export type PlayersListQueryError = ErrorType<InternalErrorResponse>;

export function createPlayersList<
	TData = Awaited<ReturnType<typeof playersList>>,
	TError = ErrorType<InternalErrorResponse>
>(
	options?: {
		query?: Partial<CreateQueryOptions<Awaited<ReturnType<typeof playersList>>, TError, TData>>;
	},
	queryClient?: QueryClient
): CreateQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> } {
	const queryOptions = getPlayersListQueryOptions(options);

	const query = createQuery(queryOptions, queryClient) as CreateQueryResult<TData, TError> & {
		queryKey: DataTag<QueryKey, TData, TError>;
	};

	query.queryKey = queryOptions.queryKey;

	return query;
}

export const playersInsert = (playersInsertRequest: BodyType<PlayersInsertRequest>) => {
	return customInstance<EntitiesPlayer>({
		url: `/players`,
		method: "POST",
		headers: { "Content-Type": "application/json" },
		data: playersInsertRequest
	});
};

export const getPlayersInsertMutationOptions = <
	TError = ErrorType<InternalErrorResponse>,
	TContext = unknown
>(options?: {
	mutation?: CreateMutationOptions<
		Awaited<ReturnType<typeof playersInsert>>,
		TError,
		{ data: BodyType<PlayersInsertRequest> },
		TContext
	>;
}): CreateMutationOptions<
	Awaited<ReturnType<typeof playersInsert>>,
	TError,
	{ data: BodyType<PlayersInsertRequest> },
	TContext
> => {
	const mutationKey = ["playersInsert"];
	const { mutation: mutationOptions } = options
		? options.mutation && "mutationKey" in options.mutation && options.mutation.mutationKey
			? options
			: { ...options, mutation: { ...options.mutation, mutationKey } }
		: { mutation: { mutationKey } };

	const mutationFn: MutationFunction<
		Awaited<ReturnType<typeof playersInsert>>,
		{ data: BodyType<PlayersInsertRequest> }
	> = (props) => {
		const { data } = props ?? {};

		return playersInsert(data);
	};

	return { mutationFn, ...mutationOptions };
};

export type PlayersInsertMutationResult = NonNullable<Awaited<ReturnType<typeof playersInsert>>>;
export type PlayersInsertMutationBody = BodyType<PlayersInsertRequest>;
export type PlayersInsertMutationError = ErrorType<InternalErrorResponse>;

export const createPlayersInsert = <TError = ErrorType<InternalErrorResponse>, TContext = unknown>(
	options?: {
		mutation?: CreateMutationOptions<
			Awaited<ReturnType<typeof playersInsert>>,
			TError,
			{ data: BodyType<PlayersInsertRequest> },
			TContext
		>;
	},
	queryClient?: QueryClient
): CreateMutationResult<
	Awaited<ReturnType<typeof playersInsert>>,
	TError,
	{ data: BodyType<PlayersInsertRequest> },
	TContext
> => {
	const mutationOptions = getPlayersInsertMutationOptions(options);

	return createMutation(mutationOptions, queryClient);
};
export const playersDelete = (id: number) => {
	return customInstance<void>({ url: `/players/${id}`, method: "DELETE" });
};

export const getPlayersDeleteMutationOptions = <
	TError = ErrorType<InternalErrorResponse>,
	TContext = unknown
>(options?: {
	mutation?: CreateMutationOptions<
		Awaited<ReturnType<typeof playersDelete>>,
		TError,
		{ id: number },
		TContext
	>;
}): CreateMutationOptions<
	Awaited<ReturnType<typeof playersDelete>>,
	TError,
	{ id: number },
	TContext
> => {
	const mutationKey = ["playersDelete"];
	const { mutation: mutationOptions } = options
		? options.mutation && "mutationKey" in options.mutation && options.mutation.mutationKey
			? options
			: { ...options, mutation: { ...options.mutation, mutationKey } }
		: { mutation: { mutationKey } };

	const mutationFn: MutationFunction<Awaited<ReturnType<typeof playersDelete>>, { id: number }> = (
		props
	) => {
		const { id } = props ?? {};

		return playersDelete(id);
	};

	return { mutationFn, ...mutationOptions };
};

export type PlayersDeleteMutationResult = NonNullable<Awaited<ReturnType<typeof playersDelete>>>;

export type PlayersDeleteMutationError = ErrorType<InternalErrorResponse>;

export const createPlayersDelete = <TError = ErrorType<InternalErrorResponse>, TContext = unknown>(
	options?: {
		mutation?: CreateMutationOptions<
			Awaited<ReturnType<typeof playersDelete>>,
			TError,
			{ id: number },
			TContext
		>;
	},
	queryClient?: QueryClient
): CreateMutationResult<
	Awaited<ReturnType<typeof playersDelete>>,
	TError,
	{ id: number },
	TContext
> => {
	const mutationOptions = getPlayersDeleteMutationOptions(options);

	return createMutation(mutationOptions, queryClient);
};
