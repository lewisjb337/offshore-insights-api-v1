using OffshoreInsights.Application.Features.Weather;
using OffshoreInsights.Domain.Entities;

namespace OffshoreInsights.Application.Features.Buoys.Responses;

public class BuoyPositionResponse : BuoyResponse
{
    public BuoyPositionResponse() { }

    public BuoyPositionResponse(Buoy buoy, WeatherConditions? weather = null) : base(buoy)
    {
        CurrentPosition = buoy.CurrentPosition is not null
            ? new BuoyTelemetryResponse(buoy.CurrentPosition)
            : null;
        Weather = weather;
    }

    public BuoyTelemetryResponse? CurrentPosition { get; set; }
    public WeatherConditions? Weather { get; set; }
}
