using MediatR;
using OffshoreInsights.Application.Features.Geofences.Abstractions;
using OffshoreInsights.Application.Features.Geofences.Queries;

namespace OffshoreInsights.Application.Features.Geofences.Handlers;

public class DeleteGeofenceHandler(IGeofencesData geofencesData) : IRequestHandler<DeleteGeofenceCommand>
{
    public async Task Handle(DeleteGeofenceCommand command, CancellationToken cancellationToken)
    {
        await geofencesData.DeleteGeofenceAsync(command.Request, cancellationToken);
    }
}
