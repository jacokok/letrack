namespace LeTrack.DTO;

public class PlayerDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? NickName { get; set; }
    public int? TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
}
