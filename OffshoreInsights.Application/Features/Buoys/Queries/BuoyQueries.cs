using MediatR;
using OffshoreInsights.Application.Features.Buoys.Requests;
using OffshoreInsights.Application.Features.Buoys.Responses;
using OffshoreInsights.Domain.Shared;

namespace OffshoreInsights.Application.Features.Buoys.Queries;

public record GetBuoysQuery(GetBuoysRequest Request) : IRequest<PagedResponse<BuoyResponse>>;
public record GetBuoyPositionByIdQuery(GetBuoyPositionByIdRequest Request) : IRequest<BuoyPositionResponse>;
public record GetBuoyTrackByIdQuery(GetBuoyTrackByIdRequest Request) : IRequest<PagedResponse<BuoyTelemetryResponse>>;
