namespace OffshoreInsights.Application.Features.Webhooks.Requests;

public record GetWebhooksRequest
{
    public string UserId { get; init; } = string.Empty;
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 50;
}

public record RegisterWebhookRequest
{
    public string UserId { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
    public string Secret { get; init; } = string.Empty;
    public string[] Events { get; init; } = [];
}

public record DeleteWebhookRequest(Guid Id, string UserId);
