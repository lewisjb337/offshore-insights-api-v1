namespace OffshoreInsights.Domain.Entities;

public class Buoy
{
    public Guid Id { get; set; }
    public long Mmsi { get; set; }
    public string? Name { get; set; }
    public string BuoyType { get; set; } = "AtoN";
    public int? AtonTypeCode { get; set; }
    public string? AtonTypeName { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public bool VirtualAid { get; set; }
    public bool OffPosition { get; set; }
    public DateTimeOffset? LastSeen { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string? ProjectName { get; set; }
    public string? SeaArea { get; set; }
    public string? Country { get; set; }
    public DateOnly? DeploymentDate { get; set; }
    public DateOnly? RecoveryDate { get; set; }
    public double? AnchorLatitude { get; set; }
    public double? AnchorLongitude { get; set; }
    public string? BuoySubtype { get; set; }
    public string? Operator { get; set; }

    // Populated in-code from the latest BuoyPositionHistory row
    public BuoyPositionHistory? CurrentPosition { get; set; }
}
