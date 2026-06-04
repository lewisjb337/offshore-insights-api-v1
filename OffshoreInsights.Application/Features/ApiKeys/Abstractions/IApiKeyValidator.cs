namespace OffshoreInsights.Application.Features.ApiKeys.Abstractions;

public interface IApiKeyValidator
{
    /// <summary>
    /// Validates a raw API key against the database.
    /// Hashes the key before lookup and checks it is active and not expired.
    /// Updates <c>LastUsedAt</c> on a successful validation.
    /// </summary>
    /// <param name="rawKey">The key exactly as received from the request header.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns><c>true</c> if the key is valid and active; otherwise <c>false</c>.</returns>
    Task<bool> ValidateAsync(string rawKey, CancellationToken cancellationToken = default);
}
