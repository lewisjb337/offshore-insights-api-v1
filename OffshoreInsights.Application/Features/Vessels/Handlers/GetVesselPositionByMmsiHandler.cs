using MediatR;
using OffshoreInsights.Application.Features.Vessels.Abstractions;
using OffshoreInsights.Application.Features.Vessels.Queries;
using OffshoreInsights.Application.Features.Vessels.Responses;
using OffshoreInsights.Application.Features.Weather;

namespace OffshoreInsights.Application.Features.Vessels.Handlers;

public class GetVesselPositionByMmsiHandler(IVesselsData vesselsData, IWeatherService weatherService)
    : IRequestHandler<GetVesselPositionByMmsiQuery, VesselPositionResponse>
{
    public async Task<VesselPositionResponse> Handle(GetVesselPositionByMmsiQuery query, CancellationToken cancellationToken)
    {
        var vessel = await vesselsData.GetVesselPositionByMmsiAsync(query.Request, cancellationToken);

        WeatherConditions? weather = null;
        if (vessel.CurrentPosition is { Latitude: not null, Longitude: not null })
            weather = await weatherService.GetConditionsAsync(
                vessel.CurrentPosition.Latitude.Value,
                vessel.CurrentPosition.Longitude.Value,
                cancellationToken);

        return new VesselPositionResponse(vessel, weather);
    }
}
