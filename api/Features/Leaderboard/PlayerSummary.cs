namespace LeTrack.Features.Leaderboard;

public class PlayerSummary
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NickName { get; set; } = string.Empty;
    public int Laps { get; set; }
    public int Rank { get; set; }
}