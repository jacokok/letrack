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
                sum(case when l.is_valid then 1 else 0 end) laps
            FROM player p
            LEFT JOIN lap l
                ON p.id = l.player_id
            GROUP BY p.id 
            ORDER BY laps DESC;
        """
        ).ToListAsync(ct);

        var teams = await _dbContext.Database.SqlQuery<TeamSummary>(
        $"""
            SELECT t.id, t.name, count(*) laps 
            FROM team t
            JOIN lap l
            ON t.id = l.team_id
            WHERE l.is_valid = true
            GROUP BY t.id
            ORDER BY laps DESC;
        """
        ).ToListAsync(ct);


        Response response = new() { PlayerSummary = players, TeamSummary = teams };

        await SendAsync(response);
    }
}