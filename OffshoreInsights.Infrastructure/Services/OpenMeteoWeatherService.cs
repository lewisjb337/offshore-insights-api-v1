using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using OffshoreInsights.Application.Features.Weather;

namespace OffshoreInsights.Infrastructure.Services;

public class OpenMeteoWeatherService(IHttpClientFactory httpClientFactory, ILogger<OpenMeteoWeatherService> logger) : IWeatherService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
    };

    public async Task<WeatherConditions?> GetConditionsAsync(double latitude, double longitude, CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient("OpenMeteo");

        var atmosphericTask = FetchAtmosphericAsync(client, latitude, longitude, cancellationToken);
        var marineTask      = FetchMarineAsync(client, latitude, longitude, cancellationToken);

        await Task.WhenAll(atmosphericTask, marineTask);

        var atm    = atmosphericTask.Result;
        var marine = marineTask.Result;

        if (atm is null && marine is null)
            return null;

        return new WeatherConditions
        {
            AirTemperatureCelsius        = atm?.Current?.Temperature2M,
            WindSpeedKnots               = atm?.Current?.WindSpeed10M,
            WindDirectionDegrees         = atm?.Current?.WindDirection10M,
            WindGustsKnots               = atm?.Current?.WindGusts10M,
            RelativeHumidityPercent      = atm?.Current?.RelativeHumidity2M,
            SeaLevelPressureHpa          = atm?.Current?.SurfacePressure,
            VisibilityMetres             = atm?.Current?.Visibility,
            WeatherCode                  = atm?.Current?.WeatherCode,
            CloudCoverPercent            = atm?.Current?.CloudCover,
            WaveHeightMetres             = marine?.Current?.WaveHeight,
            WaveDirectionDegrees         = marine?.Current?.WaveDirection,
            WavePeriodSeconds            = marine?.Current?.WavePeriod,
            SwellHeightMetres            = marine?.Current?.SwellWaveHeight,
            SwellDirectionDegrees        = marine?.Current?.SwellWaveDirection,
            SwellPeriodSeconds           = marine?.Current?.SwellWavePeriod,
            SeaSurfaceTemperatureCelsius = marine?.Current?.SeaSurfaceTemperature,
            OceanCurrentVelocityKnots    = marine?.Current?.OceanCurrentVelocity,
            OceanCurrentDirectionDegrees = marine?.Current?.OceanCurrentDirection,
            ObservedAt                   = atm?.Current?.Time is not null
                ? DateTimeOffset.Parse(atm.Current.Time + "Z")
                : null,
        };
    }

    private async Task<AtmosphericResponse?> FetchAtmosphericAsync(HttpClient client, double lat, double lon, CancellationToken ct)
    {
        const string fields = "temperature_2m,wind_speed_10m,wind_direction_10m,wind_gusts_10m," +
                              "relative_humidity_2m,surface_pressure,visibility,weather_code,cloud_cover";

        var url = $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}" +
                  $"&current={fields}&wind_speed_unit=kn&timezone=UTC";

        try
        {
            var json = await client.GetStringAsync(url, ct);
            return JsonSerializer.Deserialize<AtmosphericResponse>(json, JsonOptions);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to fetch atmospheric weather for ({Lat},{Lon})", lat, lon);
            return null;
        }
    }

    private async Task<MarineResponse?> FetchMarineAsync(HttpClient client, double lat, double lon, CancellationToken ct)
    {
        const string fields = "wave_height,wave_direction,wave_period," +
                              "swell_wave_height,swell_wave_direction,swell_wave_period," +
                              "sea_surface_temperature,ocean_current_velocity,ocean_current_direction";

        var url = $"https://marine-api.open-meteo.com/v1/marine?latitude={lat}&longitude={lon}" +
                  $"&current={fields}&wind_speed_unit=kn&timezone=UTC";

        try
        {
            var json = await client.GetStringAsync(url, ct);
            return JsonSerializer.Deserialize<MarineResponse>(json, JsonOptions);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to fetch marine weather for ({Lat},{Lon})", lat, lon);
            return null;
        }
    }

    // ── Deserialization models ────────────────────────────────────────────────

    private record AtmosphericResponse(AtmosphericCurrent? Current);

    private record AtmosphericCurrent(
        string? Time,
        [property: JsonPropertyName("temperature_2m")]       double? Temperature2M,
        [property: JsonPropertyName("wind_speed_10m")]       double? WindSpeed10M,
        [property: JsonPropertyName("wind_direction_10m")]   double? WindDirection10M,
        [property: JsonPropertyName("wind_gusts_10m")]       double? WindGusts10M,
        [property: JsonPropertyName("relative_humidity_2m")] double? RelativeHumidity2M,
        [property: JsonPropertyName("surface_pressure")]     double? SurfacePressure,
        [property: JsonPropertyName("visibility")]           double? Visibility,
        [property: JsonPropertyName("weather_code")]         int?    WeatherCode,
        [property: JsonPropertyName("cloud_cover")]          double? CloudCover);

    private record MarineResponse(MarineCurrent? Current);

    private record MarineCurrent(
        [property: JsonPropertyName("wave_height")]               double? WaveHeight,
        [property: JsonPropertyName("wave_direction")]            double? WaveDirection,
        [property: JsonPropertyName("wave_period")]               double? WavePeriod,
        [property: JsonPropertyName("swell_wave_height")]         double? SwellWaveHeight,
        [property: JsonPropertyName("swell_wave_direction")]      double? SwellWaveDirection,
        [property: JsonPropertyName("swell_wave_period")]         double? SwellWavePeriod,
        [property: JsonPropertyName("sea_surface_temperature")]   double? SeaSurfaceTemperature,
        [property: JsonPropertyName("ocean_current_velocity")]    double? OceanCurrentVelocity,
        [property: JsonPropertyName("ocean_current_direction")]   double? OceanCurrentDirection);
}
