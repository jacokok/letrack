using LeTrack.Data;
using LeTrack.Entities;
using Microsoft.EntityFrameworkCore;

namespace LeTrack.Features.Teams.Delete;

public class Endpoint : Endpoint<Request>
{
    private readonly AppDbContext _dbContext;

    public Endpoint(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Delete("/teams/{id}");
        AllowAnonymous();
    }
    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var team = await _dbContext.Team.FirstOrDefaultAsync(x => x.Id == req.Id, ct);
        if (team == null)
        {
            throw new Exception("Team not found");
        }

        _dbContext.Team.Remove(team);
        await _dbContext.SaveChangesAsync();
        await Send.NoContentAsync();
    }
}