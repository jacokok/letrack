using LeTrack.Data;
using Microsoft.EntityFrameworkCore;

namespace LeTrack.Features.Leaderboard.Lap;

public class Endpoint : Endpoint<Request, List<FastestLap>>
{
  private readonly AppDbContext _dbContext;

  public Endpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Get("/leaderboard/lap/{trackId}");
    AllowAnonymous();
  }
  public override async Task HandleAsync(Request r, CancellationToken ct)
  {
    var results = await _dbContext.Database.SqlQuery<FastestLap>(
    $"""
      SELECT
        l.track_id,
        l.lap_time,
        p.name,
        p.nick_name
      FROM lap l
      JOIN player p ON p.id = l.player_id
      WHERE 
        l.track_id = {r.TrackId}
        AND l.race_id IS NOT NULL
        AND l.is_flagged = false
        AND l.is_valid = true
      ORDER BY l.lap_time ASC
      LIMIT 10;
    """
    ).ToListAsync(ct);

    await Send.OkAsync(results);
  }
}