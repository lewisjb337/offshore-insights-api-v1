using OffshoreInsights.Domain.Enums;

namespace OffshoreInsights.Application.Features.ApiKeys.Models;

/// <summary>
/// Result returned by <see cref="Abstractions.IApiKeyValidator.ValidateAsync"/>.
/// </summary>
public record ApiKeyValidationResult(
    bool IsValid,
    string? UserId = null,
    AccountPlan Plan = AccountPlan.Free,
    bool IsRateLimited = false,
    long ApiKeyId = 0
);
