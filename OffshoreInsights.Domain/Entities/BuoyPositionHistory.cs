namespace OffshoreInsights.Domain.Entities;

public class BuoyPositionHistory
{
    public long Id { get; set; }
    public long BuoyId { get; set; }
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
