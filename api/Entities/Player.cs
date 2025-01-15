namespace LeTrack.Entities;

public class Player
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? NickName { get; set; }
    public int? TeamId { get; set; }
    public Team? Team { get; set; }
}