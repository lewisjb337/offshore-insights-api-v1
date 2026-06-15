using MediatR;
using Microsoft.AspNetCore.Mvc;
using OffshoreInsights.API.Attributes;
using OffshoreInsights.API.Helpers;
using OffshoreInsights.Application.Features.Vessels.Queries;
using OffshoreInsights.Application.Features.Vessels.Requests;
using OffshoreInsights.Application.Features.Vessels.Responses;
using OffshoreInsights.Domain.Enums;
using OffshoreInsights.Domain.Shared;

namespace OffshoreInsights.API.Controllers;

[ApiController]
[RequireApiKey]
public class VesselsController(ISender sender) : BaseController
{
    /// <summary>
    /// Retrieves details for a single vessel by its Maritime Mobile Service Identity (MMSI) number.
    /// </summary>
    /// <param name="mmsi">The 9-digit MMSI number uniquely identifying the vessel.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>The vessel matching the provided MMSI, or 404 if not found.</returns>
    [HttpGet("{mmsi:long}")]
    [RequireMinPlan(AccountPlan.Starter)]
    public async Task<IActionResult> GetVesselByMmsiAsync([FromRoute] long mmsi, CancellationToken cancellationToken)
    {
        try
        {
            var response = await sender.Send(new GetVesselByMmsiQuery(new GetVesselByMmsiRequest(mmsi)), cancellationToken);

            return Ok(ApiResponse<VesselResponse>.Ok(response));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<VesselResponse>.Fail(ex.Message));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<VesselResponse>.Fail(ex.Message));
        }
    }

    /// <summary>
    /// Retrieves a filtered, paginated list of vessel details.
    /// Accepts an optional list of MMSIs for a targeted batch lookup, or type and wind
    /// relevance filters for broader queries. All parameters are optional and composable.
    /// </summary>
    /// <param name="request">Optional filter parameters: mmsi list, vessel type, offshore wind relevance, page, and page size.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A paginated collection of vessels matching the specified filters.</returns>
    [HttpGet]
    [RequireMinPlan(AccountPlan.Starter)]
    public async Task<IActionResult> GetVesselsAsync([FromQuery] GetVesselsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await sender.Send(new GetVesselsQuery(request), cancellationToken);

            return Ok(ApiResponse<PagedResponse<VesselResponse>>.Ok(response));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<PagedResponse<VesselResponse>>.Fail(ex.Message));
        }
    }

    /// <summary>
    /// Retrieves the historic position track for a vessel over a requested time range.
    /// At least one of <c>from</c> or <c>to</c> must be provided.
    /// </summary>
    /// <param name="mmsi">The 9-digit MMSI number uniquely identifying the vessel.</param>
    /// <param name="request">Time range and pagination parameters.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A paginated list of position snapshots ordered newest first, or 404 if the vessel is not found.</returns>
    [HttpGet("{mmsi:long}/track")]
    [RequireMinPlan(AccountPlan.Starter)]
    public async Task<IActionResult> GetVesselTrackByMmsiAsync([FromRoute] long mmsi, [FromQuery] GetVesselTrackByMmsiRequest request, CancellationToken cancellationToken)
    {
        // Resolve a preset period into an absolute from value (custom from/to always win).
        if (request.Period.HasValue && !request.From.HasValue)
            request = request with { From = TrackPeriodHelper.ToFromUtc(request.Period.Value) };

        if (!request.From.HasValue && !request.To.HasValue)
            return BadRequest(ApiResponse<PagedResponse<VesselTrackPointResponse>>.Fail(
                "A time range is required. Select a preset period (e.g. Last24Hours) or provide 'from' and/or 'to'."));

        try
        {
            var response = await sender.Send(new GetVesselTrackByMmsiQuery(request with { Mmsi = mmsi }), cancellationToken);

            return Ok(ApiResponse<PagedResponse<VesselTrackPointResponse>>.Ok(response));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<PagedResponse<VesselTrackPointResponse>>.Fail(ex.Message));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<PagedResponse<VesselTrackPointResponse>>.Fail(ex.Message));
        }
    }

    /// <summary>
    /// Retrieves the current position for a single vessel by its Maritime Mobile Service Identity (MMSI) number.
    /// </summary>
    /// <param name="mmsi">The 9-digit MMSI number uniquely identifying the vessel.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>The current position of the vessel matching the provided MMSI, or 404 if not found.</returns>
    [HttpGet("{mmsi:long}/position")]
    public async Task<IActionResult> GetVesselPositionByMmsiAsync([FromRoute] long mmsi, CancellationToken cancellationToken)
    {
        try
        {
            var response = await sender.Send(new GetVesselPositionByMmsiQuery(new GetVesselPositionByMmsiRequest(mmsi)), cancellationToken);

            return Ok(ApiResponse<VesselPositionResponse>.Ok(response));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<VesselPositionResponse>.Fail(ex.Message));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<VesselPositionResponse>.Fail(ex.Message));
        }
    }

    /// <summary>
    /// Retrieves current positions for a collection of vessels.
    /// Accepts an optional list of MMSIs for a targeted batch lookup, or a geographic bounding box
    /// to return all vessels currently within that area. All parameters are optional and composable.
    /// </summary>
    /// <param name="request">Optional filter parameters: mmsi list, bounding box coordinates, page, and page size.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A paginated collection of vessel positions matching the specified filters.</returns>
    [HttpGet("positions")]
    public async Task<IActionResult> GetVesselPositionsAsync([FromQuery] GetVesselPositionsRequest request, CancellationToken cancellationToken)
    {
        var hasFilter = request.Mmsis?.Any() == true
            || request.MinLatitude.HasValue
            || request.MaxLatitude.HasValue
            || request.MinLongitude.HasValue
            || request.MaxLongitude.HasValue;

        if (!hasFilter)
            return BadRequest(ApiResponse<PagedResponse<VesselPositionResponse>>.Fail(
                "At least one filter is required: provide one or more MMSIs or a bounding box (minLatitude, maxLatitude, minLongitude, maxLongitude)."));

        try
        {
            var response = await sender.Send(new GetVesselPositionsQuery(request), cancellationToken);

            return Ok(ApiResponse<PagedResponse<VesselPositionResponse>>.Ok(response));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<PagedResponse<VesselPositionResponse>>.Fail(ex.Message));
        }
    }
}
