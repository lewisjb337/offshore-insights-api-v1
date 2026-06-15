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
    /// <summary>
    /// Retrieves a paginated list of all buoys.
    /// </summary>
    /// <param name="request">Pagination parameters: page and page size.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A paginated list of buoys.</returns>
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

    /// <summary>
    /// Retrieves the latest position and telemetry for a single buoy by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the buoy.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>The current position and telemetry of the buoy, or 404 if not found.</returns>
    [HttpGet("{id:long}/position")]
    public async Task<IActionResult> GetBuoyPositionByIdAsync([FromRoute] long id, CancellationToken cancellationToken)
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

    /// <summary>
    /// Retrieves a historic position and telemetry track for a buoy over a requested time range.
    /// At least one of <c>from</c> or <c>to</c> must be provided.
    /// </summary>
    /// <param name="id">The unique identifier of the buoy.</param>
    /// <param name="request">Time range and pagination parameters.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A paginated list of telemetry snapshots ordered newest first, or 404 if the buoy is not found.</returns>
    [HttpGet("{id:long}/track")]
    public async Task<IActionResult> GetBuoyTrackByIdAsync([FromRoute] long id, [FromQuery] GetBuoyTrackByIdRequest request, CancellationToken cancellationToken)
    {
        // Resolve a preset period into an absolute from value (custom from/to always win).
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
