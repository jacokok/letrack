using LeTrack.Entities;

namespace LeTrack.Features.Schedule;

public class Response
{
    public string Name { get; set; } = string.Empty;
    public List<RaceTrack> RaceTracks { get; set; } = new();
}