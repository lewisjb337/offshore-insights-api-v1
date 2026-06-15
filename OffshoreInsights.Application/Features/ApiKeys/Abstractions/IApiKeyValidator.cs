using OffshoreInsights.Application.Features.ApiKeys.Models;

namespace OffshoreInsights.Application.Features.ApiKeys.Abstractions;

public interface IApiKeyValidator
{
    /// <summary>
    /// Validates a raw API key, resolves the caller's plan, and enforces Free-tier call limits.
    /// </summary>
    /// <param name="rawKey">The key exactly as received from the X-Api-Key header.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>
    /// A <see cref="ApiKeyValidationResult"/> with <c>IsValid = false</c> when the key is
    /// unknown/expired, or <c>IsRateLimited = true</c> when the Free-plan monthly cap is reached.
    /// </returns>
    Task<ApiKeyValidationResult> ValidateAsync(string rawKey, CancellationToken cancellationToken = default);

    /// <summary>
    /// Records a successful API call: stamps LastUsedAt and increments the monthly usage counter.
    /// Only call this when the response status is 2xx — do not count failed/forbidden requests.
    /// Fire-and-forget safe; swallows exceptions internally.
    /// </summary>
    Task RecordUsageAsync(string userId, long apiKeyId);
}
