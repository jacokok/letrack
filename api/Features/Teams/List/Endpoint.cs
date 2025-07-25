using LeTrack.Data;
using Microsoft.EntityFrameworkCore;

namespace LeTrack.Features.Teams.List;

public class Endpoint : EndpointWithoutRequest<List<Entities.Team>>
{
    private readonly AppDbContext _dbContext;

    public Endpoint(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/teams");
        AllowAnonymous();
    }
    public override async Task HandleAsync(CancellationToken ct)
    {
        var results = await _dbContext.Team.OrderBy(x => x.Name).ToListAsync(ct);
        await Send.OkAsync(results);
    }
}