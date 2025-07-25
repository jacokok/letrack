using LeTrack.Data;
using LeTrack.DTO;
using Microsoft.EntityFrameworkCore;

namespace LeTrack.Features.Admin.ClearAll;

public class Endpoint : EndpointWithoutRequest<bool>
{
    private readonly AppDbContext _dbContext;

    public Endpoint(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Post("/admin/clear-all");
        AllowAnonymous();
    }
    public override async Task HandleAsync(CancellationToken ct)
    {

        await _dbContext.Database.ExecuteSqlAsync(
        $"""
            TRUNCATE TABLE event;
            TRUNCATE TABLE lap;
            TRUNCATE TABLE race_track;
            DELETE FROM race;         
            DELETE FROM player;
            DELETE FROM team;
        """
        );

        await Send.OkAsync(true);
    }
}