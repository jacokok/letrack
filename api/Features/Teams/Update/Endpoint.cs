using LeTrack.Data;
using LeTrack.Entities;
using Microsoft.EntityFrameworkCore;

namespace LeTrack.Features.Teams.Update;

public class Endpoint : Endpoint<Request, Team>
{
    private readonly AppDbContext _dbContext;

    public Endpoint(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Put("/teams");
        AllowAnonymous();
    }
    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var team = await _dbContext.Team.FirstOrDefaultAsync(x => x.Id == req.Id, ct);
        if (team == null)
        {
            throw new Exception("Team not found");
        }

        team.Name = req.Name;

        await _dbContext.SaveChangesAsync();
        await Send.OkAsync(team);
    }
}