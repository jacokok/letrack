namespace LeTrack.Features.Leaderboard.Player;

public class Response
{
    public int RaceId { get; set; }
    public string RaceName { get; set; } = string.Empty;
    public int TrackId { get; set; }
    public int Laps { get; set; }
}