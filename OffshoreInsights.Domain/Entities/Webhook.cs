using OffshoreInsights.Domain.Enums;

namespace OffshoreInsights.Domain.Entities;

public class Webhook
{
    public long Id { get; set; }

    /// <summary>The HTTPS endpoint that will receive event payloads.</summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>Optional shared secret used to sign outgoing payloads for verification.</summary>
    public string? Secret { get; set; }

    public WebhookDeliveryStatus DeliveryStatus { get; set; } = WebhookDeliveryStatus.Active;
    public DateTime? LastDeliveredAt { get; set; }
    public int FailureCount { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
