namespace OffshoreInsights.Application.Features.Weather;

public class WeatherConditions
{
    // ── Atmospheric ──────────────────────────────────────────────────────────
    public double? AirTemperatureCelsius { get; set; }
    public double? WindSpeedKnots { get; set; }
    public double? WindDirectionDegrees { get; set; }
    public double? WindGustsKnots { get; set; }
    public double? RelativeHumidityPercent { get; set; }
    public double? SeaLevelPressureHpa { get; set; }
    public double? VisibilityMetres { get; set; }
    public int? WeatherCode { get; set; }
    public double? CloudCoverPercent { get; set; }

    // ── Wave ─────────────────────────────────────────────────────────────────
    public double? WaveHeightMetres { get; set; }
    public double? WaveDirectionDegrees { get; set; }
    public double? WavePeriodSeconds { get; set; }

    // ── Swell ─────────────────────────────────────────────────────────────────
    public double? SwellHeightMetres { get; set; }
    public double? SwellDirectionDegrees { get; set; }
    public double? SwellPeriodSeconds { get; set; }

    // ── Ocean ─────────────────────────────────────────────────────────────────
    public double? SeaSurfaceTemperatureCelsius { get; set; }
    public double? OceanCurrentVelocityKnots { get; set; }
    public double? OceanCurrentDirectionDegrees { get; set; }

    public DateTimeOffset? ObservedAt { get; set; }
}
