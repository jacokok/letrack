using LeTrack.Data;
using Microsoft.EntityFrameworkCore;

namespace LeTrack.Features.Players.Get;

public class Endpoint : Endpoint<Request, Entities.Player>
{
    private readonly AppDbContext _dbContext;

    public Endpoint(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/players/{id}");
        AllowAnonymous();
    }
    public override async Task HandleAsync(Request r, CancellationToken ct)
    {
        var results = await _dbContext.Player
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Include(x => x.Team)
            .FirstOrDefaultAsync(x => x.Id == r.Id, ct);

        if (results == null)
        {
            await Send.NotFoundAsync();
            return;
        }

        await Send.OkAsync(results);
    }
}