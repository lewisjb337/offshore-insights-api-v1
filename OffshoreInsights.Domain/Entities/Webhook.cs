namespace OffshoreInsights.Domain.Entities;

public class Webhook
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Url { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public string[] Events { get; set; } = [];
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
