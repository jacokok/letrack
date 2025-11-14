using LeTrack.Data;
using LeTrack.Entities;
using Microsoft.EntityFrameworkCore;

namespace LeTrack.Features.Schedule;

public class Endpoint : Endpoint<Request, List<Response>>
{
    private readonly AppDbContext _dbContext;

    public Endpoint(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Post("/schedule");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        // Get teams ordered by Order column
        var teams = await _dbContext.Team
            .AsNoTracking()
            .OrderBy(t => t.Order)
            .ToListAsync(ct);

        if (teams.Count == 0)
        {
            await Send.OkAsync(new List<Response>(), ct);
            return;
        }

        // Get players ordered by Order column, grouped by team
        var players = await _dbContext.Player
            .AsNoTracking()
            .Where(p => p.TeamId != null)
            .OrderBy(p => p.Order)
            .ToListAsync(ct);

        var playersByTeam = players
            .GroupBy(p => p.TeamId!.Value)
            .ToDictionary(g => g.Key, g => g.ToList());

        // Number of tracks (assuming 4 tracks)
        const int numberOfTracks = 4;

        // Calculate the number of sessions
        var totalMinutes = req.DurationHours * 60;
        var numberOfSessions = totalMinutes / req.IntervalMinutes;

        var schedule = new List<Response>();
        var currentTime = req.StartTime;

        // Track how many times each player has raced (to determine their track rotation)
        var playerRaceCount = new Dictionary<int, int>();

        for (int session = 0; session < numberOfSessions; session++)
        {
            var endTime = currentTime.Add(TimeSpan.FromMinutes(req.IntervalMinutes));

            var raceName = $"{currentTime:hh\\:mm} - {endTime:hh\\:mm}";

            // Build assignments for this session
            var sessionAssignments = new List<(int preferredTrack, int playerId, Player player)>();

            // Determine which teams and players race in this session
            for (int track = 1; track <= numberOfTracks; track++)
            {
                // Calculate which team goes on this track for this session
                var teamIndex = (session + track - 1) % teams.Count;
                var team = teams[teamIndex];

                // Get players for this team
                if (playersByTeam.TryGetValue(team.Id, out var teamPlayers) && teamPlayers.Count > 0)
                {
                    // Calculate which player from the team races in this session
                    var playerIndex = session % teamPlayers.Count;
                    var player = teamPlayers[playerIndex];

                    // Calculate preferred track based on how many times this player has raced
                    // This ensures each player rotates through all tracks
                    int raceCount = playerRaceCount.GetValueOrDefault(player.Id, 0);
                    int preferredTrack = (raceCount % numberOfTracks) + 1;

                    sessionAssignments.Add((preferredTrack, player.Id, player));

                    // Increment race count for this player
                    playerRaceCount[player.Id] = raceCount + 1;
                }
            }

            // Resolve conflicts: ensure no two players get the same track
            var raceTracks = new List<RaceTrack>();
            var usedTracks = new HashSet<int>();

            foreach (var (preferredTrack, playerId, player) in sessionAssignments)
            {
                int assignedTrack = preferredTrack;

                // If preferred track is taken, find next available
                if (usedTracks.Contains(assignedTrack))
                {
                    // Find the next available track
                    for (int offset = 1; offset < numberOfTracks; offset++)
                    {
                        int candidateTrack = ((assignedTrack - 1 + offset) % numberOfTracks) + 1;
                        if (!usedTracks.Contains(candidateTrack))
                        {
                            assignedTrack = candidateTrack;
                            break;
                        }
                    }
                }

                usedTracks.Add(assignedTrack);
                raceTracks.Add(new RaceTrack
                {
                    TrackId = assignedTrack,
                    PlayerId = playerId,
                    Player = player
                });
            }

            schedule.Add(new Response
            {
                Name = raceName,
                RaceTracks = raceTracks
            });

            currentTime = endTime;
        }        // Save to database if requested
        if (req.Save)
        {
            var startDateTime = DateTime.UtcNow.Date.Add(req.StartTime);
            var sessionNumber = 1;

            foreach (var session in schedule)
            {
                // Parse the time range from the session name
                var times = session.Name.Split(" - ");
                var sessionStart = TimeSpan.Parse(times[0]);
                var sessionEnd = TimeSpan.Parse(times[1]);

                // Create first race for tracks 1 and 2
                var race1 = new Entities.Race
                {
                    Name = $"Session {sessionNumber:D2}: {times[0]} - {times[1]}",
                    CreatedDateTime = DateTime.UtcNow,
                    IsActive = false
                };

                _dbContext.Race.Add(race1);
                await _dbContext.SaveChangesAsync(ct);

                // Add race tracks for tracks 1 and 2
                var tracks12 = session.RaceTracks.Where(rt => rt.TrackId <= 2).ToList();
                foreach (var raceTrack in tracks12)
                {
                    _dbContext.RaceTrack.Add(new RaceTrack
                    {
                        RaceId = race1.Id,
                        TrackId = raceTrack.TrackId,
                        PlayerId = raceTrack.PlayerId
                    });
                }

                // Create second race for tracks 3 and 4
                var race2 = new Entities.Race
                {
                    Name = $"Session {sessionNumber:D2}: {times[0]} - {times[1]}",
                    CreatedDateTime = DateTime.UtcNow,
                    IsActive = false
                };

                _dbContext.Race.Add(race2);
                await _dbContext.SaveChangesAsync(ct);

                // Add race tracks for tracks 3 and 4
                var tracks34 = session.RaceTracks.Where(rt => rt.TrackId > 2).ToList();
                foreach (var raceTrack in tracks34)
                {
                    _dbContext.RaceTrack.Add(new RaceTrack
                    {
                        RaceId = race2.Id,
                        TrackId = raceTrack.TrackId,
                        PlayerId = raceTrack.PlayerId
                    });
                }

                await _dbContext.SaveChangesAsync(ct);
                sessionNumber++;
            }
        }

        await Send.OkAsync(schedule, ct);
    }
}