using OffshoreInsights.Domain.Entities;
using OffshoreInsights.Domain.Enums;

namespace OffshoreInsights.Application.Features.Vessels.Responses;

public class CurrentPositionResponse
{
    public CurrentPositionResponse() { }

    internal CurrentPositionResponse(VesselCurrentPosition position)
    {
        Latitude          = position.Latitude;
        Longitude         = position.Longitude;
        Sog               = position.Sog;
        Cog               = position.Cog;
        Heading           = position.Heading;
        Rot               = position.Rot;
        NavStatus         = position.NavStatus;
        PositionTimestamp = position.PositionTimestamp;
        Draught           = position.Draught;
        Destination       = position.Destination;
        Eta               = position.Eta;
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

    /// <summary>Rate of turn in degrees per minute (positive = turning right, negative = turning left).</summary>
    public double? Rot { get; set; }

    public NavStatus? NavStatus { get; set; }
    public DateTime? PositionTimestamp { get; set; }

    // ─── Voyage (AIS Message 5) ───────────────────────────────────────────────

    /// <summary>Current draught in metres as self-reported by the vessel.</summary>
    public double? Draught { get; set; }

    /// <summary>Destination as broadcast via AIS (free-text).</summary>
    public string? Destination { get; set; }

    /// <summary>Estimated time of arrival as broadcast via AIS (UTC).</summary>
    public DateTime? Eta { get; set; }
}
