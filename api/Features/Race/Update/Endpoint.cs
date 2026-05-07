using LeTrack.Data;
using LeTrack.Entities;
using Microsoft.EntityFrameworkCore;

namespace LeTrack.Features.Race.Update;

public class Endpoint(AppDbContext dbContext) : Endpoint<Request, Entities.Race>
{
    private readonly AppDbContext _dbContext = dbContext;

    public override void Configure()
    {
        Put("/race");
        AllowAnonymous();
    }
    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        if (req.Players.Count == 0)
        {
            throw new Exception("No players provided");
        }

        if (req.Players.Count != 4)
        {
            throw new Exception("Only four lanes supported for now");
        }

        Entities.Race? race = await _dbContext.Race.FirstOrDefaultAsync(x => x.Id == req.Id, ct) ?? throw new Exception("Race not found");
        var prevRaceTracks = await _dbContext.RaceTrack.Where(x => x.RaceId == req.Id).ToListAsync(ct);
        _dbContext.RaceTrack.RemoveRange(prevRaceTracks);


        List<RaceTrack> raceTracks = [.. req.Players.Select((x, i) => new RaceTrack { PlayerId = x, TrackId = i + 1 })];

        race.RaceTracks = raceTracks;
        race.Name = req.Name;

        await _dbContext.SaveChangesAsync(ct);
        await Send.OkAsync(race, ct);
    }
}
