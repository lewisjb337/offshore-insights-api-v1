using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OffshoreInsights.Application.Features.ApiKeys.Abstractions;
using OffshoreInsights.Persistence.Contexts;

namespace OffshoreInsights.Infrastructure.Services;

public class ApiKeyValidator(
    ILogger<ApiKeyValidator> logger,
    ApplicationDbContext context,
    IDbContextFactory<ApplicationDbContext> contextFactory) : IApiKeyValidator
{
    public async Task<bool> ValidateAsync(string rawKey, CancellationToken cancellationToken = default)
    {
        var hash = ComputeHash(rawKey);

        var apiKey = await context.ApiKeys
            .AsNoTracking()
            .FirstOrDefaultAsync(
                k => k.KeyHash == hash && k.IsActive && (k.ExpiresAt == null || k.ExpiresAt > DateTime.UtcNow),
                cancellationToken);

        if (apiKey is null)
        {
            logger.LogWarning("API key validation failed for prefix '{Prefix}'",
                rawKey.Length >= 8 ? rawKey[..8] : rawKey);
            return false;
        }

        // Stamp LastUsedAt in the background using a dedicated context so we never
        // compete with the request-scoped context that the controller will use next.
        _ = UpdateLastUsedAsync(apiKey.Id);

        return true;
    }

    private async Task UpdateLastUsedAsync(long apiKeyId)
    {
        try
        {
            // Create a fresh, independent context — avoids "A command is already in progress"
            // when the scoped request context is concurrently used by the controller action.
            await using var db = await contextFactory.CreateDbContextAsync();

            await db.ApiKeys
                .Where(k => k.Id == apiKeyId)
                .ExecuteUpdateAsync(s => s.SetProperty(k => k.LastUsedAt, DateTime.UtcNow));
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to update LastUsedAt for API key ID {Id}", apiKeyId);
        }
    }

    private static string ComputeHash(string rawKey)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawKey));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
