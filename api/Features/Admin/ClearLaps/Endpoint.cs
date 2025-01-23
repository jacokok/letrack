using LeTrack.Data;
using LeTrack.DTO;
using Microsoft.EntityFrameworkCore;

namespace LeTrack.Features.Admin.ClearLaps;

public class Endpoint : EndpointWithoutRequest<bool>
{
    private readonly AppDbContext _dbContext;

    public Endpoint(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Post("/admin/clear-laps");
        AllowAnonymous();
    }
    public override async Task HandleAsync(CancellationToken ct)
    {

        await _dbContext.Database.ExecuteSqlAsync(
        $"""
            TRUNCATE TABLE lap;
        """
        );

        await SendAsync(true);
    }
}