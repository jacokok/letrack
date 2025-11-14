<script lang="ts">
	import { Button, Card, Input, Label } from "@kayord/ui";
	import { toast } from "@kayord/ui/sonner";
	import { CalendarIcon, ClockIcon, TimerIcon, SaveIcon, XIcon } from "@lucide/svelte";
	import { createSchedule, type ScheduleResponse, type EntitiesRaceTrack } from "$lib/api";

	let startTime = $state("11:00");
	let intervalMinutes = $state(60);
	let durationHours = $state(24);
	let generatedSchedule = $state<ScheduleResponse[]>([]);
	let isGenerating = $state(false);
	let showPreview = $state(false);

	const scheduleMutation = createSchedule();

	async function generateSchedule() {
		isGenerating = true;
		generatedSchedule = [];

		try {
			// Parse start time to TimeSpan format (HH:mm:ss)
			const [hours, minutes] = startTime.split(":").map(Number);
			const timeSpan = `${hours.toString().padStart(2, "0")}:${minutes.toString().padStart(2, "0")}:00`;

			const response = await scheduleMutation.mutateAsync({
				data: {
					startTime: timeSpan,
					intervalMinutes: intervalMinutes,
					durationHours: durationHours,
					save: false
				}
			});

			generatedSchedule = response;
			showPreview = true;

			const totalRaces = response.reduce((sum, session) => sum + session.raceTracks.length, 0);
			toast.success(`Generated ${response.length} sessions with ${totalRaces} races`);
		} catch (error) {
			toast.error("Failed to generate schedule");
			console.error(error);
		} finally {
			isGenerating = false;
		}
	}

	async function saveSchedule() {
		try {
			// Parse start time to TimeSpan format (HH:mm:ss)
			const [hours, minutes] = startTime.split(":").map(Number);
			const timeSpan = `${hours.toString().padStart(2, "0")}:${minutes.toString().padStart(2, "0")}:00`;

			await scheduleMutation.mutateAsync({
				data: {
					startTime: timeSpan,
					intervalMinutes: intervalMinutes,
					durationHours: durationHours,
					save: true
				}
			});

			toast.success("Schedule saved successfully!");
			discardSchedule();
		} catch (error) {
			toast.error("Failed to save schedule");
			console.error(error);
		}
	}

	function discardSchedule() {
		generatedSchedule = [];
		showPreview = false;
		toast.info("Schedule discarded");
	}

	function groupByTrack(raceTracks: EntitiesRaceTrack[]) {
		return [...raceTracks].sort((a, b) => a.trackId - b.trackId);
	}
</script>

<div class="m-2">
	<div class="mb-4">
		<h1 class="text-2xl font-bold">Race Schedule Generator</h1>
		<p class="text-muted-foreground">
			Create a race schedule by specifying start time, interval, and duration
		</p>
	</div>

	{showPreview}

	<Card.Root class="mb-4">
		<Card.Header>
			<Card.Title>Schedule Parameters</Card.Title>
			<Card.Description>Configure the race schedule settings</Card.Description>
		</Card.Header>
		<Card.Content>
			<div class="grid gap-4 md:grid-cols-3">
				<div class="space-y-2">
					<Label for="start-time" class="flex items-center gap-2">
						<ClockIcon class="h-4 w-4" />
						Start Time
					</Label>
					<Input
						id="start-time"
						type="time"
						bind:value={startTime}
						placeholder="11:00"
						disabled={isGenerating}
					/>
				</div>

				<div class="space-y-2">
					<Label for="interval" class="flex items-center gap-2">
						<TimerIcon class="h-4 w-4" />
						Interval (minutes)
					</Label>
					<Input
						id="interval"
						type="number"
						bind:value={intervalMinutes}
						min="5"
						max="60"
						disabled={isGenerating}
					/>
				</div>

				<div class="space-y-2">
					<Label for="duration" class="flex items-center gap-2">
						<CalendarIcon class="h-4 w-4" />
						Duration (hours)
					</Label>
					<Input
						id="duration"
						type="number"
						bind:value={durationHours}
						min="1"
						max="12"
						disabled={isGenerating}
					/>
				</div>
			</div>
		</Card.Content>
		<Card.Footer>
			<Button onclick={generateSchedule} disabled={isGenerating} class="w-full">
				{isGenerating ? "Generating..." : "Generate Schedule"}
			</Button>
		</Card.Footer>
	</Card.Root>

	{#if showPreview && generatedSchedule.length > 0}
		<Card.Root>
			<Card.Header class="flex flex-row items-center justify-between">
				<div>
					<Card.Title>Schedule Preview</Card.Title>
					<Card.Description>
						{generatedSchedule.length} sessions • {generatedSchedule.reduce(
							(sum, s) => sum + s.raceTracks.length,
							0
						)} total races
					</Card.Description>
				</div>
				<div class="flex gap-2">
					<Button onclick={discardSchedule} variant="outline" size="sm">
						<XIcon class="mr-2 h-4 w-4" />
						Discard
					</Button>
					<Button onclick={saveSchedule} size="sm">
						<SaveIcon class="mr-2 h-4 w-4" />
						Save Schedule
					</Button>
				</div>
			</Card.Header>
			<Card.Content>
				<div class="space-y-4">
					{#each generatedSchedule as session, index (session.name)}
						<div class="rounded-lg border p-4">
							<div class="mb-3 flex items-center justify-between">
								<h3 class="font-semibold">Session {index + 1}</h3>
								<span class="text-sm text-muted-foreground">
									{session.name}
								</span>
							</div>
							<div class="grid gap-2 md:grid-cols-2 lg:grid-cols-4">
								{#each groupByTrack(session.raceTracks) as raceTrack (raceTrack.trackId)}
									<div class="rounded-md border bg-card p-3">
										<div class="text-sm font-medium">Track {raceTrack.trackId}</div>
										<div class="text-sm text-muted-foreground">
											{raceTrack.player.name}
										</div>
									</div>
								{/each}
							</div>
						</div>
					{/each}
				</div>
			</Card.Content>
		</Card.Root>
	{/if}
</div>
