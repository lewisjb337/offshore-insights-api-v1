using OffshoreInsights.Application.Features.Weather;
using OffshoreInsights.Domain.Entities;

namespace OffshoreInsights.Application.Features.Vessels.Responses;

public class VesselPositionResponse : VesselResponse
{
    public VesselPositionResponse() { }

    public VesselPositionResponse(Vessel vessel, WeatherConditions? weather = null) : base(vessel)
    {
        CurrentPosition = vessel.CurrentPosition is not null
            ? new CurrentPositionResponse(vessel.CurrentPosition)
            : null;
        Weather = weather;
    }

    public CurrentPositionResponse? CurrentPosition { get; set; }
    public WeatherConditions? Weather { get; set; }
}
