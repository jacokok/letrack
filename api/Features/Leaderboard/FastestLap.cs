namespace LeTrack.Features.Leaderboard;

public class FastestLap
{
    public int TrackId { get; set; }
    public TimeSpan LapTime { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? NickName { get; set; }
}