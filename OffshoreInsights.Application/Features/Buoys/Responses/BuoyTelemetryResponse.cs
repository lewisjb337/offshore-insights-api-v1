using OffshoreInsights.Domain.Entities;

namespace OffshoreInsights.Application.Features.Buoys.Responses;

/// <summary>
/// A single position/telemetry snapshot from BuoyPositionHistory.
/// </summary>
public class BuoyTelemetryResponse
{
    public BuoyTelemetryResponse() { }

    internal BuoyTelemetryResponse(BuoyPositionHistory h)
    {
        Latitude              = h.Latitude;
        Longitude             = h.Longitude;
        OffPosition           = h.OffPosition;
        ReceivedAt            = h.ReceivedAt;
        AnchorLat             = h.AnchorLat;
        AnchorLon             = h.AnchorLon;
        DistanceFromAnchorNm  = h.DistanceFromAnchorNm;
    }

    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool OffPosition { get; set; }
    public DateTimeOffset ReceivedAt { get; set; }
    public double? AnchorLat { get; set; }
    public double? AnchorLon { get; set; }
    public double? DistanceFromAnchorNm { get; set; }
}
