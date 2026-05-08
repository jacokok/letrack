using LeTrack.Data;
using Microsoft.EntityFrameworkCore;

namespace LeTrack.Features.Race.Clear;

public class Endpoint(AppDbContext dbContext) : Endpoint<Request>
{
    private readonly AppDbContext _dbContext = dbContext;

    public override void Configure()
    {
        Post("/race/clear");
        AllowAnonymous();
    }
    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var race = await _dbContext.Race.FirstOrDefaultAsync(x => x.Id == req.Id, ct) ?? throw new Exception("Race not found");
        var laps = await _dbContext.Lap.Where(x => x.RaceId == req.Id).ToListAsync(ct);
        _dbContext.Lap.RemoveRange(laps);
        race.EndDateTime = null;
        race.IsActive = false;
        race.TimeRemaining = null;
        race.RestartDateTime = null;
        race.StartDateTime = null;
        await _dbContext.SaveChangesAsync(ct);
        await Send.NoContentAsync(ct);
    }
}
