namespace LeTrack.Features.Players.Insert;

public class Request
{
    public string Name { get; set; } = string.Empty;
    public string? NickName { get; set; }
    public int TeamId { get; set; }
    public int Order { get; set; }
}