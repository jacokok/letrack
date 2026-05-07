using LeTrack.Data;
using LeTrack.DTO;
using Microsoft.EntityFrameworkCore;

namespace LeTrack.Features.Race.Summary;

public class Endpoint(AppDbContext dbContext) : Endpoint<Request, Response>
{
    private readonly AppDbContext _dbContext = dbContext;

    public override void Configure()
    {
        Get("/race/summary/{raceId}");
        AllowAnonymous();
    }
    public override async Task HandleAsync(Request r, CancellationToken ct)
    {
        Response response = new();

        var race = await _dbContext.Race
            .AsNoTracking()
            .Where(x => x.Id == r.RaceId)
            .Select(x => new RaceDTO
            {
                Id = x.Id,
                Name = x.Name,
                IsActive = x.IsActive,
                CreatedDateTime = x.CreatedDateTime,
                StartDateTime = x.StartDateTime,
                RestartDateTime = x.RestartDateTime,
                EndDateTime = x.EndDateTime,
                EndLapCount = x.EndLapCount,
                TimeRemaining = x.TimeRemaining,
            })
            .FirstOrDefaultAsync(ct) ?? throw new Exception("Race not found");

        var raceTracks = await _dbContext.RaceTrack
            .AsNoTracking()
            .Where(rt => rt.RaceId == r.RaceId)
            .OrderBy(rt => rt.TrackId)
            .Select(rt => new
            {
                rt.TrackId,
                Player = new PlayerDTO
                {
                    Id = rt.Player.Id,
                    Name = rt.Player.Name,
                    NickName = rt.Player.NickName,
                    TeamId = rt.Player.TeamId,
                    TeamName = rt.Player.Team != null ? rt.Player.Team.Name : "",
                },
            })
            .ToListAsync(ct);

        var allLaps = await _dbContext.Lap
            .AsNoTracking()
            .Where(l => l.RaceId == r.RaceId)
            .OrderBy(l => l.TrackId)
            .ThenBy(l => l.Timestamp)
            .ThenBy(l => l.Id)
            .Select(l => new LapDTO
            {
                Id = l.Id,
                LastLapId = l.LastLapId,
                TrackId = l.TrackId,
                Timestamp = l.Timestamp,
                LapTime = l.LapTime,
                LapTimeDifference = l.LapTimeDifference,
                IsFlagged = l.IsFlagged,
                FlagReason = l.FlagReason,
                RaceId = l.RaceId,
                PlayerId = l.PlayerId,
                TeamId = l.TeamId,
                IsValid = l.IsValid,
            })
            .ToListAsync(ct);

        var lapsByTrack = allLaps
            .GroupBy(l => l.TrackId)
            .ToDictionary(g => g.Key, g => g.ToList());

        response.Tracks = [.. raceTracks
            .Select(rt =>
            {
                var laps = lapsByTrack.GetValueOrDefault(rt.TrackId, []);

                for (var i = 0; i < laps.Count; i++)
                    laps[i].LapNumber = i + 1;

                return new Track
                {
                    TrackId = rt.TrackId,
                    Player = rt.Player,
                    TotalLaps = laps.Count,
                    FastestLap = laps
                        .Where(l => !l.IsFlagged && l.LapTime != null)
                        .MinBy(l => l.LapTime),
                    Laps = [.. laps.TakeLast(10).Reverse()],
                };
            })];

        response.Race = race;
        await Send.OkAsync(response, ct);
    }
}
