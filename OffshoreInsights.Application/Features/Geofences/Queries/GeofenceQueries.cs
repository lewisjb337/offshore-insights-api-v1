using MediatR;
using OffshoreInsights.Application.Features.Geofences.Requests;
using OffshoreInsights.Application.Features.Geofences.Responses;
using OffshoreInsights.Domain.Shared;

namespace OffshoreInsights.Application.Features.Geofences.Queries;

public record GetGeofencesQuery(GetGeofencesRequest Request) : IRequest<PagedResponse<GeofenceResponse>>;
public record CreateGeofenceCommand(CreateGeofenceRequest Request) : IRequest<GeofenceResponse>;
public record DeleteGeofenceCommand(DeleteGeofenceRequest Request) : IRequest;
public record GetGeofenceEventsQuery(GetGeofenceEventsRequest Request) : IRequest<PagedResponse<GeofenceEventResponse>>;
