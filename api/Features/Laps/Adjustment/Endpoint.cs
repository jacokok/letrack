using LeTrack.Data;
using LeTrack.Entities;
using Microsoft.EntityFrameworkCore;

namespace LeTrack.Features.Laps.Adjustment;

public class Endpoint : Endpoint<Request>
{
    private readonly AppDbContext _dbContext;

    public Endpoint(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Post("/laps/adjustment");
        AllowAnonymous();
    }
    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var racePlayer = await _dbContext.RaceTrack
            .Where(x => x.RaceId == req.RaceId && x.TrackId == req.TrackId)
            .Include(x => x.Player)
            .FirstOrDefaultAsync(ct);

        if (racePlayer == null)
        {
            throw new Exception("Player not found");
        }

        for (int i = 0; i < req.Amount; i++)
        {
            var lap = new Lap
            {
                Id = Guid.NewGuid(),
                TrackId = req.TrackId,
                Timestamp = DateTime.UtcNow,
                LastLapId = null,
                LapTime = null,
                LapTimeDifference = null,
                IsFlagged = true,
                RaceId = req.RaceId,
                FlagReason = "Adjustment",
                PlayerId = racePlayer.PlayerId,
                TeamId = racePlayer.Player.TeamId,
            };
            await _dbContext.Lap.AddAsync(lap, ct);
        }

        await _dbContext.SaveChangesAsync(ct);
        await SendNoContentAsync(ct);
    }
}