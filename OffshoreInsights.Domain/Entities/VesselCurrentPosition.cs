using OffshoreInsights.Domain.Enums;

namespace OffshoreInsights.Domain.Entities;

public class VesselCurrentPosition
{
    public long Id { get; set; }
    public long VesselId { get; set; }

    // ─── Position (AIS Messages 1/2/3) ───────────────────────────────────────

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public double? Sog { get; set; }
    public double? Cog { get; set; }
    public double? Heading { get; set; }

    /// <summary>Rate of turn (raw AIS integer, smallint in DB).</summary>
    public short? Rot { get; set; }

    public NavStatus? NavStatus { get; set; }
    public DateTime? PositionTimestamp { get; set; }

    // ─── Voyage data (AIS Message 5) ─────────────────────────────────────────

    /// <summary>Current draught in metres as self-reported by the vessel.</summary>
    public double? Draught { get; set; }

    /// <summary>Destination as broadcast via AIS (free-text, often a port name or UN/LOCODE).</summary>
    public string? Destination { get; set; }

    /// <summary>Estimated time of arrival as broadcast via AIS.</summary>
    public DateTimeOffset? Eta { get; set; }
}
