namespace LeTrack.Features.Schedule;

public class Request
{
    public TimeSpan StartTime { get; set; }
    public int IntervalMinutes { get; set; }
    public int DurationHours { get; set; }
    public bool Save { get; set; }
}