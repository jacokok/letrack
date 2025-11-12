using LeTrack.Data;
using Microsoft.EntityFrameworkCore;

namespace LeTrack.Features.Teams.Get;

public class Endpoint : Endpoint<Request, Entities.Team>
{
    private readonly AppDbContext _dbContext;

    public Endpoint(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/teams/{id}");
        AllowAnonymous();
    }
    public override async Task HandleAsync(Request r, CancellationToken ct)
    {
        var results = await _dbContext.Team.FirstOrDefaultAsync(x => x.Id == r.Id, ct);
        if (results == null)
        {
            await Send.NotFoundAsync();
            return;
        }
        await Send.OkAsync(results);
    }
}