using MediatR;
using Microsoft.AspNetCore.Mvc;
using OffshoreInsights.API.Attributes;
using OffshoreInsights.Application.Features.Webhooks.Queries;
using OffshoreInsights.Application.Features.Webhooks.Requests;
using OffshoreInsights.Application.Features.Webhooks.Responses;
using OffshoreInsights.Domain.Enums;
using OffshoreInsights.Domain.Shared;

namespace OffshoreInsights.API.Controllers;

[ApiController]
[RequireApiKey]
[RequireMinPlan(AccountPlan.Professional)]
public class WebhooksController(ISender sender) : BaseController
{
    /// <summary>
    /// Retrieves a paginated list of registered webhooks for the account.
    /// </summary>
    /// <param name="request">Pagination parameters: page and page size.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A paginated list of registered webhooks.</returns>
    [HttpGet]
    public async Task<IActionResult> GetWebhooksAsync([FromQuery] GetWebhooksRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var userId   = HttpContext.Items["UserId"] as string ?? string.Empty;
            var response = await sender.Send(new GetWebhooksQuery(request with { UserId = userId }), cancellationToken);

            return Ok(ApiResponse<PagedResponse<WebhookResponse>>.Ok(response));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<PagedResponse<WebhookResponse>>.Fail(ex.Message));
        }
    }

    /// <summary>
    /// Registers a webhook endpoint URL to receive geofence and alert event notifications.
    /// </summary>
    /// <param name="request">The webhook URL, signing secret, and event types to subscribe to.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>The newly registered webhook.</returns>
    [HttpPost]
    public async Task<IActionResult> RegisterWebhookAsync([FromBody] RegisterWebhookRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var userId   = HttpContext.Items["UserId"] as string ?? string.Empty;
            var response = await sender.Send(new RegisterWebhookCommand(request with { UserId = userId }), cancellationToken);

            return CreatedAtAction(nameof(GetWebhooksAsync), ApiResponse<WebhookResponse>.Ok(response));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<WebhookResponse>.Fail(ex.Message));
        }
    }

    /// <summary>
    /// Removes a registered webhook endpoint and stops all future deliveries to it.
    /// </summary>
    /// <param name="id">The unique identifier of the webhook to remove.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>204 No Content on success, or 404 if the webhook is not found.</returns>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteWebhookAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var userId = HttpContext.Items["UserId"] as string ?? string.Empty;
            await sender.Send(new DeleteWebhookCommand(new DeleteWebhookRequest(id, userId)), cancellationToken);

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
}
