using OffshoreInsights.Domain.Entities;

namespace OffshoreInsights.Application.Features.Webhooks.Responses;

public class WebhookResponse
{
    public WebhookResponse() { }

    internal WebhookResponse(Webhook w)
    {
        Id        = w.Id;
        Url       = w.Url;
        Events    = w.Events;
        IsActive  = w.IsActive;
        CreatedAt = w.CreatedAt;
        UpdatedAt = w.UpdatedAt;
    }

    public Guid Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string[] Events { get; set; } = [];
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public static implicit operator WebhookResponse(Webhook w) => new(w);
}
