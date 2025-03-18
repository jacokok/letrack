using LeTrack.Data;
using LeTrack.DTO;
using Microsoft.EntityFrameworkCore;

namespace LeTrack.Features.Admin.Snapshot;

public class Endpoint : EndpointWithoutRequest<bool>
{
    private readonly AppDbContext _dbContext;

    public Endpoint(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Post("/admin/snapshot");
        AllowAnonymous();
    }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "EF1002:Risk of vulnerability to SQL injection.", Justification = "Cannot create db without raw")]
    public override async Task HandleAsync(CancellationToken ct)
    {
        string dbName = $"letrack_{DateTime.Now:yyyyMMddHHmmss}";

        await _dbContext.Database.ExecuteSqlAsync(
            $"""
                SELECT pg_terminate_backend(pg_stat_activity.pid) FROM pg_stat_activity 
                WHERE pg_stat_activity.datname = 'letrack' AND pid <> pg_backend_pid();
            """
        , ct);


        _ = await _dbContext.Database.ExecuteSqlRawAsync(
        $"""
            CREATE DATABASE {dbName}
            WITH TEMPLATE letrack;
        """
        , ct);


        await SendAsync(true);
    }
}