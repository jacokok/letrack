using LeTrack.Data;
using LeTrack.DTO;
using Microsoft.EntityFrameworkCore;

namespace LeTrack.Features.Laps;

public class Endpoint : Endpoint<Request, List<LapDTO>>
{
    private readonly AppDbContext _dbContext;

    public Endpoint(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/laps");
        AllowAnonymous();
    }
    public override async Task HandleAsync(Request r, CancellationToken ct)
    {
        var laps = await _dbContext.Lap
            .Where(x => x.RaceId == r.RaceId && x.TrackId == r.TrackId)
            .OrderBy(x => x.Timestamp)
            .ProjectToDto()
            .ToListAsync(ct);

        laps = laps.Select((x, index) => { x.LapNumber = index + 1; return x; }).ToList();

        await Send.OkAsync(laps);
    }
}