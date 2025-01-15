using LeTrack.Data;
using Microsoft.EntityFrameworkCore;

namespace LeTrack.Features.Scoreboard;

public class Endpoint : EndpointWithoutRequest<List<Entities.Lap>>
{
    private readonly AppDbContext _dbContext;

    public Endpoint(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/scoreboard");
        AllowAnonymous();
    }
    public override async Task HandleAsync(CancellationToken ct)
    {
        var laps = await _dbContext.Lap
            .Include(x => x.Player)
            .Include(x => x.Race)
            .Include(x => x.Team)
            .Where(x => x.IsValid)
            .ToListAsync(ct);

        await SendAsync(laps);
    }
}