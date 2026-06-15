using MediatR;
using Microsoft.AspNetCore.Mvc;
using OffshoreInsights.API.Attributes;
using OffshoreInsights.Application.Features.Geofences.Queries;
using OffshoreInsights.Application.Features.Geofences.Requests;
using OffshoreInsights.Application.Features.Geofences.Responses;
using OffshoreInsights.Domain.Enums;
using OffshoreInsights.Domain.Shared;

namespace OffshoreInsights.API.Controllers;

[ApiController]
[RequireApiKey]
[RequireMinPlan(AccountPlan.Professional)]
public class GeofenceController(ISender sender) : BaseController
{
    /// <summary>
    /// Retrieves a paginated list of all geofences for the account.
    /// </summary>
    /// <param name="request">Pagination parameters: page and page size.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A paginated list of geofences.</returns>
    [HttpGet]
    public async Task<IActionResult> GetGeofencesAsync([FromQuery] GetGeofencesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await sender.Send(new GetGeofencesQuery(request), cancellationToken);

            return Ok(ApiResponse<PagedResponse<GeofenceResponse>>.Ok(response));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<PagedResponse<GeofenceResponse>>.Fail(ex.Message));
        }
    }

    /// <summary>
    /// Creates a new geofence zone with a polygon or circle geometry and an optional list of target vessels.
    /// </summary>
    /// <param name="request">The geofence definition including name, type, geometry, and target MMSIs.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>The newly created geofence.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateGeofenceAsync([FromBody] CreateGeofenceRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await sender.Send(new CreateGeofenceCommand(request), cancellationToken);

            return CreatedAtAction(nameof(GetGeofencesAsync), ApiResponse<GeofenceResponse>.Ok(response));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<GeofenceResponse>.Fail(ex.Message));
        }
    }

    /// <summary>
    /// Deletes a geofence and stops all associated alert delivery.
    /// </summary>
    /// <param name="id">The unique identifier of the geofence to delete.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>204 No Content on success, or 404 if the geofence is not found.</returns>
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteGeofenceAsync([FromRoute] long id, CancellationToken cancellationToken)
    {
        try
        {
            await sender.Send(new DeleteGeofenceCommand(new DeleteGeofenceRequest(id)), cancellationToken);

            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<object>.Fail(ex.Message));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
    }

    /// <summary>
    /// Retrieves entry and exit events for a geofence over a time range.
    /// </summary>
    /// <param name="id">The unique identifier of the geofence.</param>
    /// <param name="request">Optional time range and pagination parameters.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A paginated list of geofence events ordered newest first, or 404 if the geofence is not found.</returns>
    [HttpGet("{id:long}/events")]
    public async Task<IActionResult> GetGeofenceEventsAsync([FromRoute] long id, [FromQuery] GetGeofenceEventsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await sender.Send(new GetGeofenceEventsQuery(request with { Id = id }), cancellationToken);

            return Ok(ApiResponse<PagedResponse<GeofenceEventResponse>>.Ok(response));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<PagedResponse<GeofenceEventResponse>>.Fail(ex.Message));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<PagedResponse<GeofenceEventResponse>>.Fail(ex.Message));
        }
    }
}
