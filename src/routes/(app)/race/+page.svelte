<script lang="ts">
	import { Button, Card, Pagination } from "@kayord/ui";
	import PlusIcon from "lucide-svelte/icons/plus";
	import { createRaceList } from "$lib/api";
	import Race from "./Race.svelte";
	import AddRace from "./AddRace.svelte";

	let open = $state(false);

	let page = $state(1);
	const pageSize = 20;

	const query = $derived(createRaceList({ page: page, pageSize: pageSize }));
</script>

<div class="m-2">
	<div class="flex items-center justify-end gap-2">
		<Button class="my-2" onclick={() => (open = true)}>
			<PlusIcon />Add Race
		</Button>
	</div>

	{#if $query.data?.items?.length == 0}
		<Card.Root class="p-2">No Races</Card.Root>
	{/if}

	<div class="flex flex-col gap-2">
		{#each $query.data?.items ?? [] as race}
			<Race {race} refetch={$query.refetch} />
		{/each}
	</div>

	<AddRace bind:open refetch={$query.refetch} />

	{#if ($query.data?.totalPages ?? 0) > 1}
		<Pagination.Root count={$query.data?.totalCount ?? 0} perPage={pageSize} bind:page class="mt-2">
			{#snippet children({ pages, currentPage })}
				<Pagination.Content>
					<Pagination.Item>
						<Pagination.PrevButton />
					</Pagination.Item>
					{#each pages as page (page.key)}
						{#if page.type === "ellipsis"}
							<Pagination.Item>
								<Pagination.Ellipsis />
							</Pagination.Item>
						{:else}
							<Pagination.Item>
								<Pagination.Link {page} isActive={currentPage === page.value}>
									{page.value}
								</Pagination.Link>
							</Pagination.Item>
						{/if}
					{/each}
					<Pagination.Item>
						<Pagination.NextButton />
					</Pagination.Item>
				</Pagination.Content>
			{/snippet}
		</Pagination.Root>
	{/if}
</div>
