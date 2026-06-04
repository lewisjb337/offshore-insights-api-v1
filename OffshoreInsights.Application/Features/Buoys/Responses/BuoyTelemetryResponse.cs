using OffshoreInsights.Domain.Entities;

namespace OffshoreInsights.Application.Features.Buoys.Responses;

/// <summary>
/// A single telemetry snapshot — used for both current position and track history points.
/// </summary>
public class BuoyTelemetryResponse
{
    public BuoyTelemetryResponse() { }

    internal BuoyTelemetryResponse(BuoyCurrentPosition position)
    {
        Latitude                = position.Latitude;
        Longitude               = position.Longitude;
        WaterTemperatureCelsius = position.WaterTemperatureCelsius;
        AirTemperatureCelsius   = position.AirTemperatureCelsius;
        WindSpeedKnots          = position.WindSpeedKnots;
        WindDirectionDegrees    = position.WindDirectionDegrees;
        WaveHeightMetres        = position.WaveHeightMetres;
        WavePeriodSeconds       = position.WavePeriodSeconds;
        AirPressureHpa          = position.AirPressureHpa;
        PositionTimestamp       = position.PositionTimestamp;
    }

    internal BuoyTelemetryResponse(BuoyPositionHistory history)
    {
        Latitude                = history.Latitude;
        Longitude               = history.Longitude;
        WaterTemperatureCelsius = history.WaterTemperatureCelsius;
        AirTemperatureCelsius   = history.AirTemperatureCelsius;
        WindSpeedKnots          = history.WindSpeedKnots;
        WindDirectionDegrees    = history.WindDirectionDegrees;
        WaveHeightMetres        = history.WaveHeightMetres;
        WavePeriodSeconds       = history.WavePeriodSeconds;
        AirPressureHpa          = history.AirPressureHpa;
        PositionTimestamp       = history.PositionTimestamp;
    }

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public double? WaterTemperatureCelsius { get; set; }
    public double? AirTemperatureCelsius { get; set; }
    public double? WindSpeedKnots { get; set; }
    public double? WindDirectionDegrees { get; set; }
    public double? WaveHeightMetres { get; set; }
    public double? WavePeriodSeconds { get; set; }
    public double? AirPressureHpa { get; set; }
    public DateTime? PositionTimestamp { get; set; }
}
