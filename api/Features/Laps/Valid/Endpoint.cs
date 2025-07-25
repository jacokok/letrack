using LeTrack.Data;
using Microsoft.EntityFrameworkCore;

namespace LeTrack.Features.Laps.Valid;

public class Endpoint : Endpoint<Request, bool>
{
    private readonly AppDbContext _dbContext;

    public Endpoint(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Post("/laps/valid");
        AllowAnonymous();
    }
    public override async Task HandleAsync(Request r, CancellationToken ct)
    {
        var laps = await _dbContext.Lap.Where(x => r.Ids.Contains(x.Id)).ToListAsync(ct);
        foreach (var lap in laps)
        {
            lap.IsValid = !lap.IsValid;
        }

        await _dbContext.SaveChangesAsync();
        await Send.OkAsync(true);
    }
}