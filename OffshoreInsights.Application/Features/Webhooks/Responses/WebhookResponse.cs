using OffshoreInsights.Domain.Entities;
using OffshoreInsights.Domain.Enums;

namespace OffshoreInsights.Application.Features.Webhooks.Responses;

public class WebhookResponse
{
    public WebhookResponse() { }

    internal WebhookResponse(Webhook webhook)
    {
        Id             = webhook.Id;
        Url            = webhook.Url;
        DeliveryStatus = webhook.DeliveryStatus;
        LastDeliveredAt = webhook.LastDeliveredAt;
        FailureCount   = webhook.FailureCount;
        CreatedAt      = webhook.CreatedAt;
    }

    public long Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public WebhookDeliveryStatus DeliveryStatus { get; set; }
    public DateTime? LastDeliveredAt { get; set; }
    public int FailureCount { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public static implicit operator WebhookResponse(Webhook webhook) => new(webhook);
}
