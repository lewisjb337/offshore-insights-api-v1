using MediatR;
using OffshoreInsights.Application.Features.Buoys.Abstractions;
using OffshoreInsights.Application.Features.Buoys.Queries;
using OffshoreInsights.Application.Features.Buoys.Responses;
using OffshoreInsights.Application.Features.Weather;

namespace OffshoreInsights.Application.Features.Buoys.Handlers;

public class GetBuoyPositionByIdHandler(IBuoysData buoysData, IWeatherService weatherService)
    : IRequestHandler<GetBuoyPositionByIdQuery, BuoyPositionResponse>
{
    public async Task<BuoyPositionResponse> Handle(GetBuoyPositionByIdQuery query, CancellationToken cancellationToken)
    {
        var buoy = await buoysData.GetBuoyPositionByIdAsync(query.Request, cancellationToken);

        WeatherConditions? weather = null;
        if (buoy.CurrentPosition is not null)
            weather = await weatherService.GetConditionsAsync(
                buoy.CurrentPosition.Latitude,
                buoy.CurrentPosition.Longitude,
                cancellationToken);

        return new BuoyPositionResponse(buoy, weather);
    }
}
