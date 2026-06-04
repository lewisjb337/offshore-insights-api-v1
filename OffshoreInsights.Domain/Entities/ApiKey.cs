namespace OffshoreInsights.Domain.Entities;

public class ApiKey
{
    public long Id { get; set; }

    /// <summary>The Identity user this key belongs to.</summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>User-given label, e.g. "Production" or "CI Pipeline".</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>SHA-256 hex digest of the raw key. Never store the raw key.</summary>
    public string KeyHash { get; set; } = string.Empty;

    /// <summary>First 8 characters of the raw key — safe to display in dashboards.</summary>
    public string KeyPrefix { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
    public DateTime? ExpiresAt { get; set; }
    public DateTime? LastUsedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
