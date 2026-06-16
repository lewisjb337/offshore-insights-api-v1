namespace OffshoreInsights.Domain.Entities;

public class BuoyPositionHistory
{
    public long Id { get; set; }
    public long BuoyMmsi { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool OffPosition { get; set; }
    public DateTimeOffset ReceivedAt { get; set; }
    public string? BuoyName { get; set; }
    public double? AnchorLat { get; set; }
    public double? AnchorLon { get; set; }
    public double? DistanceFromAnchorNm { get; set; }
}
