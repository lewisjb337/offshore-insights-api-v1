using MediatR;
using Microsoft.AspNetCore.Mvc;
using OffshoreInsights.API.Attributes;
using OffshoreInsights.API.Helpers;
using OffshoreInsights.Application.Features.Buoys.Queries;
using OffshoreInsights.Application.Features.Buoys.Requests;
using OffshoreInsights.Application.Features.Buoys.Responses;
using OffshoreInsights.Domain.Enums;
using OffshoreInsights.Domain.Shared;

namespace OffshoreInsights.API.Controllers;

[ApiController]
[RequireApiKey]
[RequireMinPlan(AccountPlan.Professional)]
public class BuoysController(ISender sender) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetBuoysAsync([FromQuery] GetBuoysRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await sender.Send(new GetBuoysQuery(request), cancellationToken);
            return Ok(ApiResponse<PagedResponse<BuoyResponse>>.Ok(response));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<PagedResponse<BuoyResponse>>.Fail(ex.Message));
        }
    }

    [HttpGet("{id:guid}/position")]
    public async Task<IActionResult> GetBuoyPositionByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var response = await sender.Send(new GetBuoyPositionByIdQuery(new GetBuoyPositionByIdRequest(id)), cancellationToken);
            return Ok(ApiResponse<BuoyPositionResponse>.Ok(response));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<BuoyPositionResponse>.Fail(ex.Message));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<BuoyPositionResponse>.Fail(ex.Message));
        }
    }

    [HttpGet("{id:guid}/track")]
    public async Task<IActionResult> GetBuoyTrackByIdAsync([FromRoute] Guid id, [FromQuery] GetBuoyTrackByIdRequest request, CancellationToken cancellationToken)
    {
        if (request.Period.HasValue && !request.From.HasValue)
            request = request with { From = TrackPeriodHelper.ToFromUtc(request.Period.Value) };

        if (!request.From.HasValue && !request.To.HasValue)
            return BadRequest(ApiResponse<PagedResponse<BuoyTelemetryResponse>>.Fail(
                "A time range is required. Select a preset period (e.g. Last24Hours) or provide 'from' and/or 'to'."));

        try
        {
            var response = await sender.Send(new GetBuoyTrackByIdQuery(request with { Id = id }), cancellationToken);
            return Ok(ApiResponse<PagedResponse<BuoyTelemetryResponse>>.Ok(response));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<PagedResponse<BuoyTelemetryResponse>>.Fail(ex.Message));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<PagedResponse<BuoyTelemetryResponse>>.Fail(ex.Message));
        }
    }
}
