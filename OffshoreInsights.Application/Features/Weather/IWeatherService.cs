namespace OffshoreInsights.Application.Features.Weather;

public interface IWeatherService
{
    Task<WeatherConditions?> GetConditionsAsync(double latitude, double longitude, CancellationToken cancellationToken = default);
}
