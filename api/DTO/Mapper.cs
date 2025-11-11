using LeTrack.Entities;

namespace LeTrack.DTO;

public static class Mapper
{
    public static IQueryable<LapDTO> ProjectToDto(this IQueryable<Lap> q)
    {
        return q.Select(lap => new LapDTO
        {
            Id = lap.Id,
            LastLapId = lap.LastLapId,
            TrackId = lap.TrackId,
            Timestamp = lap.Timestamp,
            LapTime = lap.LapTime,
            LapTimeDifference = lap.LapTimeDifference,
            IsFlagged = lap.IsFlagged,
            FlagReason = lap.FlagReason,
            RaceId = lap.RaceId,
            PlayerId = lap.PlayerId,
            TeamId = lap.TeamId,
            IsValid = lap.IsValid,
        });
    }

    public static LapDTO MapLap(Lap lap)
    {
        return new LapDTO
        {
            Id = lap.Id,
            LastLapId = lap.LastLapId,
            TrackId = lap.TrackId,
            Timestamp = lap.Timestamp,
            LapTime = lap.LapTime,
            LapTimeDifference = lap.LapTimeDifference,
            IsFlagged = lap.IsFlagged,
            FlagReason = lap.FlagReason,
            RaceId = lap.RaceId,
            PlayerId = lap.PlayerId,
            TeamId = lap.TeamId,
            IsValid = lap.IsValid,
        };
    }
}