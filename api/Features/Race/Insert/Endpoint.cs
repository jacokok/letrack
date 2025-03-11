using LeTrack.Data;
using LeTrack.Entities;

namespace LeTrack.Features.Race.Insert;

public class Endpoint : Endpoint<Request, Entities.Race>
{
    private readonly AppDbContext _dbContext;

    public Endpoint(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Post("/race");
        AllowAnonymous();
    }
    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        if (req.Players.Count == 0)
        {
            throw new Exception("No players provided");
        }

        if (req.Players.Count > 2)
        {
            throw new Exception("Only two lanes supported for now");
        }

        List<RaceTrack> raceTracks = [.. req.Players.Select((x, i) => new RaceTrack { PlayerId = x, TrackId = i + 1 + (req.IsFirstTracks == true ? 0 : 2) })];

        Entities.Race race = new()
        {
            Name = req.Name,
            CreatedDateTime = DateTime.UtcNow,
            RaceTracks = raceTracks
        };
        await _dbContext.Race.AddAsync(race);
        await _dbContext.SaveChangesAsync();
        await SendAsync(race);
    }
}