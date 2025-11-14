using LeTrack.Data;
using Microsoft.EntityFrameworkCore;

namespace LeTrack.Features.Leaderboard;

public class Endpoint : EndpointWithoutRequest<Response>
{
    private readonly AppDbContext _dbContext;

    public Endpoint(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/leaderboard");
        AllowAnonymous();
    }
    public override async Task HandleAsync(CancellationToken ct)
    {
        var players = await _dbContext.Database.SqlQuery<PlayerSummary>(
        $"""
            SELECT 
                p.id, 
                p.name, 
                p.nick_name, 
                sum(case when l.is_valid then 1 else 0 end) laps,
                RANK() OVER (ORDER BY sum(case when l.is_valid then 1 else 0 end) DESC) rank
            FROM player p
            LEFT JOIN lap l
                ON p.id = l.player_id
            GROUP BY p.id 
            ORDER BY laps DESC;
        """
        ).ToListAsync(ct);

        var teams = await _dbContext.Database.SqlQuery<TeamSummary>(
        $"""
            SELECT 
                t.id, 
                t.name, 
                sum(case when l.is_valid then 1 else 0 end) laps,
                RANK() OVER (ORDER BY sum(case when l.is_valid then 1 else 0 end) DESC) rank
            FROM team t
            LEFT JOIN lap l
                ON t.id = l.team_id
            GROUP BY t.id
            ORDER BY laps DESC;
        """
        ).ToListAsync(ct);

        var fastestLap = await _dbContext.Database.SqlQuery<FastestLap>(
        $"""
            SELECT DISTINCT ON (l.track_id)
              l.track_id,
              l.lap_time,
              p.name,
              p.nick_name
            FROM lap l
            JOIN player p ON p.id = l.player_id
            WHERE 
              l.race_id IS NOT NULL
              AND l.is_flagged = false
              AND l.is_valid = true
            ORDER BY l.track_id, l.lap_time ASC;
        """
        ).ToListAsync(ct);

        Response response = new() { PlayerSummary = players, TeamSummary = teams, FastestLap = fastestLap };

        await Send.OkAsync(response);
    }
}