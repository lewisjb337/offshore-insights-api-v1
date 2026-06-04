using MediatR;
using OffshoreInsights.Application.Features.Geofences.Abstractions;
using OffshoreInsights.Application.Features.Geofences.Queries;
using OffshoreInsights.Application.Features.Geofences.Responses;

namespace OffshoreInsights.Application.Features.Geofences.Handlers;

public class CreateGeofenceHandler(IGeofencesData geofencesData) : IRequestHandler<CreateGeofenceCommand, GeofenceResponse>
{
    public async Task<GeofenceResponse> Handle(CreateGeofenceCommand command, CancellationToken cancellationToken)
    {
        var result = await geofencesData.CreateGeofenceAsync(command.Request, cancellationToken);

        return result;
    }
}
