namespace LeTrack.Features.Teams.Update;

public class Request
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }
}