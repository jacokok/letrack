/**
 * Generated by orval v7.3.0 🍺
 * Do not edit manually.
 * letrack-api
 * OpenAPI spec version: v1
 */
import { createQuery } from "@tanstack/svelte-query";
import type {
	CreateQueryOptions,
	CreateQueryResult,
	DataTag,
	QueryFunction,
	QueryKey
} from "@tanstack/svelte-query";
import type { InternalErrorResponse } from "./api.schemas";
import { customInstance } from "../mutator/customInstance.svelte";
import type { ErrorType } from "../mutator/customInstance.svelte";

export const testSignal = () => {
	return customInstance<boolean>({ url: `/test/signal`, method: "GET" });
};

export const getTestSignalQueryKey = () => {
	return [`/test/signal`] as const;
};

export const getTestSignalQueryOptions = <
	TData = Awaited<ReturnType<typeof testSignal>>,
	TError = ErrorType<InternalErrorResponse>
>(options?: {
	query?: Partial<CreateQueryOptions<Awaited<ReturnType<typeof testSignal>>, TError, TData>>;
}) => {
	const { query: queryOptions } = options ?? {};

	const queryKey = queryOptions?.queryKey ?? getTestSignalQueryKey();

	const queryFn: QueryFunction<Awaited<ReturnType<typeof testSignal>>> = () => testSignal();

	return { queryKey, queryFn, ...queryOptions } as CreateQueryOptions<
		Awaited<ReturnType<typeof testSignal>>,
		TError,
		TData
	> & { queryKey: DataTag<QueryKey, TData> };
};

export type TestSignalQueryResult = NonNullable<Awaited<ReturnType<typeof testSignal>>>;
export type TestSignalQueryError = ErrorType<InternalErrorResponse>;

export function createTestSignal<
	TData = Awaited<ReturnType<typeof testSignal>>,
	TError = ErrorType<InternalErrorResponse>
>(options?: {
	query?: Partial<CreateQueryOptions<Awaited<ReturnType<typeof testSignal>>, TError, TData>>;
}): CreateQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData> } {
	const queryOptions = getTestSignalQueryOptions(options);

	const query = createQuery(queryOptions) as CreateQueryResult<TData, TError> & {
		queryKey: DataTag<QueryKey, TData>;
	};

	query.queryKey = queryOptions.queryKey;

	return query;
}

export const test = () => {
	return customInstance<boolean>({ url: `/test`, method: "GET" });
};

export const getTestQueryKey = () => {
	return [`/test`] as const;
};

export const getTestQueryOptions = <
	TData = Awaited<ReturnType<typeof test>>,
	TError = ErrorType<InternalErrorResponse>
>(options?: {
	query?: Partial<CreateQueryOptions<Awaited<ReturnType<typeof test>>, TError, TData>>;
}) => {
	const { query: queryOptions } = options ?? {};

	const queryKey = queryOptions?.queryKey ?? getTestQueryKey();

	const queryFn: QueryFunction<Awaited<ReturnType<typeof test>>> = () => test();

	return { queryKey, queryFn, ...queryOptions } as CreateQueryOptions<
		Awaited<ReturnType<typeof test>>,
		TError,
		TData
	> & { queryKey: DataTag<QueryKey, TData> };
};

export type TestQueryResult = NonNullable<Awaited<ReturnType<typeof test>>>;
export type TestQueryError = ErrorType<InternalErrorResponse>;

export function createTest<
	TData = Awaited<ReturnType<typeof test>>,
	TError = ErrorType<InternalErrorResponse>
>(options?: {
	query?: Partial<CreateQueryOptions<Awaited<ReturnType<typeof test>>, TError, TData>>;
}): CreateQueryResult<TData, TError> & { queryKey: DataTag<QueryKey, TData> } {
	const queryOptions = getTestQueryOptions(options);

	const query = createQuery(queryOptions) as CreateQueryResult<TData, TError> & {
		queryKey: DataTag<QueryKey, TData>;
	};

	query.queryKey = queryOptions.queryKey;

	return query;
}