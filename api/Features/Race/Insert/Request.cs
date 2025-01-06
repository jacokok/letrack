namespace LeTrack.Features.Race.Insert;

public class Request
{
    public string Name { get; set; } = string.Empty;
    public List<int> Players { get; set; } = new();
}