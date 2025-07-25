using System.Globalization;
using CsvHelper;
using LeTrack.Data;
using LeTrack.DTO;
using Microsoft.EntityFrameworkCore;

namespace LeTrack.Features.Export;

public class Endpoint : EndpointWithoutRequest<List<LapExportDTO>>
{
    private readonly AppDbContext _dbContext;

    public Endpoint(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/export");
        AllowAnonymous();
    }
    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await _dbContext.Database.SqlQuery<LapExportDTO>(
        $"""
            SELECT
                l.id,
            l.track_id,
            l.timestamp,
            l.lap_time,
            l.is_flagged,
            l.last_lap_id,
            l.race_id,
            r.name race_name,
            r.start_date_time race_start_date_time,
            r.end_date_time race_end_date_time,
            l.lap_time_difference,
            l.flag_reason,
            l.player_id,
            p.name player_name,
            p.nick_name player_nick_name,
            l.is_valid,
            l.team_id,
            t.name team_name
            FROM lap l
            LEFT JOIN race r
                ON r.id = l.race_id
            LEFT JOIN player p
                ON p.id = l.player_id
            LEFT JOIN team t
                ON t.id = l.team_id
        """
        ).ToListAsync(ct);

        using MemoryStream ms = new();
        using var writer = new StreamWriter(ms);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        await csv.WriteRecordsAsync(result, ct);
        await writer.FlushAsync();
        await Send.BytesAsync(ms.ToArray(), "laps.csv", "text/csv", cancellation: ct);
        // await Send.OkAsync(result);
    }
}