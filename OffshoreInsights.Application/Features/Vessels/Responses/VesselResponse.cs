using OffshoreInsights.Domain.Entities;
using OffshoreInsights.Domain.Enums;

namespace OffshoreInsights.Application.Features.Vessels.Responses;

public class VesselResponse
{
    public VesselResponse() { }

    protected VesselResponse(Vessel vessel)
    {
        Id                     = vessel.Id;
        Mmsi                   = vessel.Mmsi;
        Imo                    = vessel.Imo == 0 ? null : vessel.Imo;
        Name                   = vessel.Name;
        Callsign               = vessel.Callsign;
        VesselType             = vessel.VesselType;
        OffshoreSubtype        = vessel.OffshoreSubtype;
        IsOffshoreWindRelevant = vessel.IsOffshoreWindRelevant;
        FirstSeen              = vessel.FirstSeen;
        LastSeen               = vessel.LastSeen;
    }

    public long Id { get; set; }
    public long Mmsi { get; set; }
    public long? Imo { get; set; }
    public string? Name { get; set; }
    public string? Callsign { get; set; }

    /// <summary>
    /// Vessel type mapped from the AIS type code (0–99) supplied by the AIS data provider.
    /// This is the most accurate classification available from the feed.
    /// </summary>
    public VesselType? VesselType { get; set; }

    /// <summary>Offshore wind-specific subtype applied by our classification layer (e.g. CTV, SOV, WTI).</summary>
    public string? OffshoreSubtype { get; set; }
    public bool? IsOffshoreWindRelevant { get; set; }
    public DateOnly? FirstSeen { get; set; }
    public DateTime? LastSeen { get; set; }

    public static implicit operator VesselResponse(Vessel vessel) => new(vessel);
}
