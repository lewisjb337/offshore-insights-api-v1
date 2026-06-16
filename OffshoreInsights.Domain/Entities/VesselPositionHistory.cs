namespace OffshoreInsights.Domain.Entities;

public class VesselPositionHistory
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

    /// <summary>NavStatus stored as the raw AIS integer to preserve full fidelity.</summary>
    public long? NavStatus { get; set; }

    public DateTime? PositionTimestamp { get; set; }

    // ─── Voyage snapshot (AIS Message 5, recorded at time of fix) ────────────

    /// <summary>Draught in metres recorded at the time of this position fix.</summary>
    public double? Draught { get; set; }

    /// <summary>Destination text recorded at the time of this position fix.</summary>
    public string? Destination { get; set; }

    /// <summary>ETA broadcast at the time of this position fix.</summary>
    public DateTimeOffset? Eta { get; set; }
}
