using OffshoreInsights.Domain.Enums;

namespace OffshoreInsights.Domain.Entities;

public class Vessel
{
    public long Id { get; set; }
    public long Mmsi { get; set; }
    public long? Imo { get; set; }
    public string? Name { get; set; }
    public string? Callsign { get; set; }
    public long? VesselTypeId { get; set; }
    public VesselType? VesselType { get; set; }
    public string? OffshoreSubtype { get; set; }
    public bool? IsOffshoreWindRelevant { get; set; }
    public DateOnly? FirstSeen { get; set; }
    public DateTime? LastSeen { get; set; }

    public VesselCurrentPosition? CurrentPosition { get; set; }
    public ICollection<VesselPositionHistory> PositionHistory { get; set; } = [];
}
