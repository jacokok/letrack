using LeTrack.DTO;

namespace LeTrack.Features.Race.Summary;

public class Response
{
    public List<Track> Tracks { get; set; } = [];
    public RaceDTO Race { get; set; } = default!;
}

public class Track
{
    public int TrackId { get; set; }
    public List<LapDTO> Laps { get; set; } = [];
    public PlayerDTO Player { get; set; } = default!;
    public LapDTO? FastestLap { get; set; }
    public int TotalLaps { get; set; }
}
