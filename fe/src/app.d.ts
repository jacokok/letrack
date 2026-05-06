import { CustomOptions, CustomColumnMeta } from "$lib/components/custom/data-table/types";
// See https://svelte.dev/docs/kit/types#app.d.ts
// for information about these interfaces
declare global {
	namespace App {
		// interface Error {}
		// interface Locals {}
		// interface PageData {}
		// interface PageState {}
		// interface Platform {}
	}
}

declare module "@tanstack/table-core" {
	// eslint-disable-next-line @typescript-eslint/no-empty-object-type, @typescript-eslint/no-unused-vars
	interface ColumnMeta<TData extends RowData, TValue> extends CustomColumnMeta {}
	// eslint-disable-next-line @typescript-eslint/no-empty-object-type, @typescript-eslint/no-unused-vars
	interface TableOptionsResolved<TData extends RowData> extends CustomOptions {}
}

export {};
