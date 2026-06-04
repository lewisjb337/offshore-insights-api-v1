using OffshoreInsights.Domain.Entities;

namespace OffshoreInsights.Application.Features.Vessels.Responses;

/// <summary>
/// A single position snapshot from a vessel's historic track, including the voyage
/// context that was active at the time of the fix.
/// </summary>
public class VesselTrackPointResponse
{
    public VesselTrackPointResponse() { }

    internal VesselTrackPointResponse(VesselPositionHistory history)
    {
        Latitude          = history.Latitude;
        Longitude         = history.Longitude;
        Sog               = history.Sog;
        Cog               = history.Cog;
        Heading           = history.Heading;
        Rot               = history.Rot;
        NavStatus         = history.NavStatus;
        PositionTimestamp = history.PositionTimestamp;
        Draught           = history.Draught;
        Destination       = history.Destination;
        Eta               = history.Eta;
    }

    // ─── Position ─────────────────────────────────────────────────────────────

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    /// <summary>Speed over ground in knots.</summary>
    public double? Sog { get; set; }

    /// <summary>Course over ground in degrees (0–360).</summary>
    public double? Cog { get; set; }

    /// <summary>True heading in degrees (0–360).</summary>
    public double? Heading { get; set; }

    /// <summary>Rate of turn in degrees per minute.</summary>
    public long? Rot { get; set; }

    /// <summary>Navigational status as the raw AIS integer value.</summary>
    public long? NavStatus { get; set; }

    public DateTime? PositionTimestamp { get; set; }

    // ─── Voyage snapshot ──────────────────────────────────────────────────────

    /// <summary>Draught in metres recorded at the time of this fix.</summary>
    public double? Draught { get; set; }

    /// <summary>AIS destination text recorded at the time of this fix.</summary>
    public string? Destination { get; set; }

    /// <summary>ETA broadcast at the time of this fix (UTC).</summary>
    public DateTime? Eta { get; set; }
}
