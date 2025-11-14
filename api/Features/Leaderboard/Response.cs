namespace LeTrack.Features.Leaderboard;

public class Response
{
    public List<PlayerSummary>? PlayerSummary { get; set; }
    public List<TeamSummary>? TeamSummary { get; set; }
    public List<FastestLap>? FastestLap { get; set; }
}