namespace OffshoreInsights.Application.Features.Webhooks.Requests;

/// <summary>
/// Retrieves a paginated list of registered webhooks and their delivery status.
/// </summary>
public record GetWebhooksRequest
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 50;
}

/// <summary>
/// Registers a new webhook endpoint to receive geofence and alert event notifications.
/// </summary>
public record RegisterWebhookRequest
{
    /// <summary>The HTTPS endpoint URL that will receive event payloads.</summary>
    public string Url { get; init; } = string.Empty;

    /// <summary>
    /// Optional shared secret used to sign outgoing payloads (HMAC-SHA256).
    /// Store securely — it will not be returned after registration.
    /// </summary>
    public string? Secret { get; init; }
}

/// <summary>
/// Removes a registered webhook endpoint.
/// </summary>
public record DeleteWebhookRequest(long Id);
