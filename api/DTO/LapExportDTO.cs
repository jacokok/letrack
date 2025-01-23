namespace LeTrack.DTO;

public class LapExportDTO
{
    public Guid Id { get; set; }
    public int TrackId { get; set; }
    public DateTime Timestamp { get; set; }
    public TimeSpan? LapTime { get; set; }
    public bool IsFlagged { get; set; }
    public Guid? LastLapId { get; set; }
    public int? RaceId { get; set; }
    public string? RaceName { get; set; }
    public DateTime? RaceStartDateTime { get; set; }
    public DateTime? RaceEndDateTime { get; set; }
    public TimeSpan? LapTimeDifference { get; set; }
    public string? FlagReason { get; set; }
    public int? PlayerId { get; set; }
    public string? PlayerName { get; set; }
    public string? PlayerNickName { get; set; }
    public bool IsValid { get; set; } = true;
    public int? TeamId { get; set; }
    public string? TeamName { get; set; }

}