namespace OffshoreInsights.Domain.Entities;

/// <summary>
/// Read-only view of the AccountSubscriptions row written by the Stripe webhook.
/// A row with Plan = "Free" is inserted by the create-api-key Edge Function when a
/// user creates their first key without a paid subscription.
/// </summary>
public class AccountSubscription
{
    public long Id { get; set; }

    /// <summary>Foreign key to auth.users (uuid stored as string).</summary>
    public string UserId { get; set; } = string.Empty;

    public string Plan { get; set; } = "Free";
    public string Status { get; set; } = "Active";

    public DateTimeOffset? CurrentPeriodStart { get; set; }
    public DateTimeOffset? CurrentPeriodEnd { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
