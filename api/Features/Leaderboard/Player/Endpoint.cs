using LeTrack.Data;
using Microsoft.EntityFrameworkCore;

namespace LeTrack.Features.Leaderboard.Player;

public class Endpoint : Endpoint<Request, List<Response>>
{
  private readonly AppDbContext _dbContext;

  public Endpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Get("/leaderboard/player/{id}");
    AllowAnonymous();
  }
  public override async Task HandleAsync(Request r, CancellationToken ct)
  {
    var results = await _dbContext.Database.SqlQuery<Response>(
    $"""
      SELECT 
        l.race_id,
        r.name race_name,
        l.track_id,
        sum(case when l.is_valid then 1 else 0 end) laps
      FROM race r
      JOIN lap l
        ON l.race_id = r.id
      WHERE l.player_id = {r.Id}
      GROUP BY 
        l.race_id,
        r.name,
        l.track_id
      ORDER BY l.race_id;
    """
    ).ToListAsync(ct);

    await Send.OkAsync(results);
  }
}