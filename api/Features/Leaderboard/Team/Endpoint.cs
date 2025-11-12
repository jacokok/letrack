using LeTrack.Data;
using Microsoft.EntityFrameworkCore;

namespace LeTrack.Features.Leaderboard.Team;

public class Endpoint : Endpoint<Request, List<Response>>
{
  private readonly AppDbContext _dbContext;

  public Endpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Get("/leaderboard/team/{id}");
    AllowAnonymous();
  }
  public override async Task HandleAsync(Request r, CancellationToken ct)
  {
    var results = await _dbContext.Database.SqlQuery<Response>(
    $"""
            SELECT 
              l.race_id,
              r.name race_name,
              sum(case when l.is_valid then 1 else 0 end) laps
            FROM race r
            JOIN lap l
              ON l.race_id = r.id
            WHERE l.team_id = {r.Id}
            GROUP BY 
              l.race_id,
              r.name
            ORDER BY l.race_id;
        """
    ).ToListAsync(ct);

    await Send.OkAsync(results);
  }
}