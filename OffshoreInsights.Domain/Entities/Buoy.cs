namespace OffshoreInsights.Domain.Entities;

public class Buoy
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Owner { get; set; }
    public string? Country { get; set; }
    public bool IsActive { get; set; }
    public DateOnly? FirstSeen { get; set; }
    public DateTime? LastSeen { get; set; }

    public BuoyCurrentPosition? CurrentPosition { get; set; }
    public ICollection<BuoyPositionHistory> PositionHistory { get; set; } = [];
}
