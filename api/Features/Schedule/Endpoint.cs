using LeTrack.Data;
using LeTrack.Entities;
using Microsoft.EntityFrameworkCore;

namespace LeTrack.Features.Schedule;

public class Endpoint(AppDbContext dbContext) : Endpoint<Request, List<Response>>
{
    private const int NumberOfTracks = 4;
    private readonly AppDbContext _dbContext = dbContext;

    private sealed record TeamPair(Player FirstRacer, Player SecondRacer);
    private sealed record SessionAssignment(int CurrentTrackId, int NextTrackId, Player FirstRacer, Player SecondRacer);

    public override void Configure()
    {
        Post("/schedule");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var teams = await _dbContext.Team
            .AsNoTracking()
            .OrderBy(t => t.Order)
            .ToListAsync(ct);

        if (teams.Count == 0)
        {
            await Send.OkAsync([], ct);
            return;
        }

        var players = await _dbContext.Player
            .AsNoTracking()
            .Where(p => p.TeamId != null)
            .OrderBy(p => p.TeamId)
            .ThenBy(p => p.Order)
            .ToListAsync(ct);

        var teamPairsByTeamId = players
            .GroupBy(p => p.TeamId!.Value)
            .ToDictionary(
                g => g.Key,
                g => BuildPairs(g.OrderBy(p => p.Order).ToList()));

        var eligibleTeams = teams
            .Where(team => teamPairsByTeamId.TryGetValue(team.Id, out var pairs) && pairs.Count > 0)
            .ToList();

        if (eligibleTeams.Count == 0)
        {
            await Send.OkAsync([], ct);
            return;
        }

        var interval = TimeSpan.FromMinutes(req.IntervalMinutes);
        var padding = TimeSpan.FromMinutes(req.PaddingMinutes);
        var scheduleEnd = req.StartTime.Add(TimeSpan.FromHours(req.DurationHours));
        var sessionDuration = TimeSpan.FromTicks((interval.Ticks * 4) + (padding.Ticks * 3));
        var activeTeamCount = Math.Min(NumberOfTracks, eligibleTeams.Count);

        var schedule = new List<Response>();
        var teamSessionCounts = new Dictionary<int, int>();
        var currentSessionStart = req.StartTime;
        var sessionNumber = 1;

        while (currentSessionStart + sessionDuration <= scheduleEnd)
        {
            var sessionAssignments = new List<SessionAssignment>(activeTeamCount);

            for (var slot = 0; slot < activeTeamCount; slot++)
            {
                var team = eligibleTeams[(sessionNumber - 1 + slot) % eligibleTeams.Count];
                var pairs = teamPairsByTeamId[team.Id];
                var teamSessionCount = teamSessionCounts.GetValueOrDefault(team.Id, 0);
                var pair = pairs[teamSessionCount % pairs.Count];
                var currentTrackId = slot + 1;
                var nextTrackId = (currentTrackId % NumberOfTracks) + 1;

                sessionAssignments.Add(new SessionAssignment(currentTrackId, nextTrackId, pair.FirstRacer, pair.SecondRacer));
                teamSessionCounts[team.Id] = teamSessionCount + 1;
            }

            var race1Start = currentSessionStart;
            var race1End = race1Start + interval;
            var race2Start = race1End + padding;
            var race2End = race2Start + interval;
            var race3Start = race2End + padding;
            var race3End = race3Start + interval;
            var race4Start = race3End + padding;
            var race4End = race4Start + interval;

            schedule.Add(new Response
            {
                Name = FormatRaceName(sessionNumber, 1, race1Start, race1End),
                RaceTracks = sessionAssignments
                    .Select(a => CreateRaceTrack(a.CurrentTrackId, a.FirstRacer))
                    .ToList()
            });

            schedule.Add(new Response
            {
                Name = FormatRaceName(sessionNumber, 2, race2Start, race2End),
                RaceTracks = sessionAssignments
                    .Select(a => CreateRaceTrack(a.CurrentTrackId, a.SecondRacer))
                    .ToList()
            });

            schedule.Add(new Response
            {
                Name = FormatRaceName(sessionNumber, 3, race3Start, race3End),
                RaceTracks = sessionAssignments
                    .Select(a => CreateRaceTrack(a.NextTrackId, a.FirstRacer))
                    .ToList()
            });

            schedule.Add(new Response
            {
                Name = FormatRaceName(sessionNumber, 4, race4Start, race4End),
                RaceTracks = sessionAssignments
                    .Select(a => CreateRaceTrack(a.NextTrackId, a.SecondRacer))
                    .ToList()
            });

            currentSessionStart = race4End + padding;
            sessionNumber++;
        }

        if (req.Save && schedule.Count > 0)
        {
            var createdDateTime = DateTime.UtcNow;

            var races = schedule.Select(race => new Entities.Race
            {
                Name = race.Name,
                CreatedDateTime = createdDateTime,
                IsActive = false,
                RaceTracks = race.RaceTracks
                    .OrderBy(rt => rt.TrackId)
                    .Select(rt => new RaceTrack
                    {
                        TrackId = rt.TrackId,
                        PlayerId = rt.PlayerId
                    })
                    .ToList()
            });

            _dbContext.Race.AddRange(races);
            await _dbContext.SaveChangesAsync(ct);
        }

        await Send.OkAsync(schedule, ct);
    }

    private static List<TeamPair> BuildPairs(List<Player> teamPlayers)
    {
        var pairs = new List<TeamPair>();

        for (var index = 0; index + 1 < teamPlayers.Count; index += 2)
        {
            pairs.Add(new TeamPair(teamPlayers[index], teamPlayers[index + 1]));
        }

        return pairs;
    }

    private static RaceTrack CreateRaceTrack(int trackId, Player player) => new()
    {
        TrackId = trackId,
        PlayerId = player.Id,
        Player = player
    };

    private static string FormatRaceName(int sessionNumber, int raceNumber, TimeSpan start, TimeSpan end) =>
        $"Session {sessionNumber} - Race {raceNumber}: {start:hh\\:mm} - {end:hh\\:mm}";
}
