using LeTrack.Data;
using LeTrack.Entities;

namespace LeTrack.Features.Teams.Insert;

public class Endpoint : Endpoint<Request, Team>
{
    private readonly AppDbContext _dbContext;

    public Endpoint(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Post("/teams");
        AllowAnonymous();
    }
    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        Team team = new() { Name = req.Name };
        await _dbContext.Team.AddAsync(team);
        await _dbContext.SaveChangesAsync();
        await Send.OkAsync(team);
    }
}