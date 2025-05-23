/**
 * Generated by orval v7.8.0 🍺
 * Do not edit manually.
 * letrack-api
 * OpenAPI spec version: v1
 */
import { createQuery } from "@tanstack/svelte-query";
import type {
	CreateQueryOptions,
	CreateQueryResult,
	DataTag,
	QueryClient,
	QueryFunction,
	QueryKey
} from "@tanstack/svelte-query";

import type { InternalErrorResponse, LeaderboardResponse } from "./api.schemas";

import { customInstance } from "../mutator/customInstance.svelte";
import type { ErrorType } from "../mutator/customInstance.svelte";

export const leaderboard = () => {
	return customInstance<LeaderboardResponse>({ url: `/leaderboard`, method: "GET" });
};

export const getLeaderboardQueryKey = () => {
	return [`/leaderboard`] as const;
};

export const getLeaderboardQueryOptions = <
	TData = Awaited<ReturnType<typeof leaderboard>>,
	TError = ErrorType<InternalErrorResponse>
>(options?: {
	query?: Partial<CreateQueryOptions<Awaited<ReturnType<typeof leaderboard>>, TError, TData>>;
}) => {
	const { query: queryOptions } = options ?? {};

	const queryKey = queryOptions?.queryKey ?? getLeaderboardQueryKey();

	const queryFn: QueryFunction<Awaited<ReturnType<typeof leaderboard>>> = () => leaderboard();

	return { queryKey, queryFn, ...queryOptions } as CreateQueryOptions<
		Awaited<ReturnType<typeof leaderboard>>,
		TError,
		TData
	> & { queryKey: DataTag<QueryKey, TData, TError> };
};

export type LeaderboardQueryResult = NonNullable<Awaited<ReturnType<typeof leaderboard>>>;
export type LeaderboardQueryError = ErrorType<InternalErrorResponse>;

export function createLeaderboard<
	TData = Awaited<ReturnType<typeof leaderboard>>,
	TError = ErrorType<InternalErrorResponse>
>(
	options?: {
		query?: Partial<CreateQueryOptions<Awaited<ReturnType<typeof leaderboard>>, TError, TData>>;
	},
	queryClient?: QueryClient
): CreateQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData, TError> } {
	const queryOptions = getLeaderboardQueryOptions(options);

	const query = createQuery(queryOptions, queryClient) as CreateQueryResult<TData, TError> & {
		queryKey: DataTag<QueryKey, TData, TError>;
	};

	query.queryKey = queryOptions.queryKey;

	return query;
}
