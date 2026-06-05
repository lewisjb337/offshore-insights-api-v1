using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OffshoreInsights.Application.Features.ApiKeys.Abstractions;
using OffshoreInsights.Domain.Entities;
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

        // Fire-and-forget both writes using a dedicated factory context so we never
        // compete with the request-scoped context that the controller will use next.
        _ = UpdateLastUsedAndIncrementUsageAsync(apiKey.Id, apiKey.UserId);

        return true;
    }

    private async Task UpdateLastUsedAndIncrementUsageAsync(long apiKeyId, string userId)
    {
        try
        {
            await using var db = await contextFactory.CreateDbContextAsync();

            // 1. Stamp LastUsedAt on the key
            await db.ApiKeys
                .Where(k => k.Id == apiKeyId)
                .ExecuteUpdateAsync(s => s.SetProperty(k => k.LastUsedAt, DateTime.UtcNow));

            // 2. Upsert call count for the current billing period (1st of this month)
            var periodStart = new DateOnly(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            var now         = DateTimeOffset.UtcNow;

            var usage = await db.ApiCallUsage
                .FirstOrDefaultAsync(u => u.UserId == userId && u.PeriodStart == periodStart);

            if (usage is null)
            {
                db.ApiCallUsage.Add(new ApiCallUsage
                {
                    UserId      = userId,
                    PeriodStart = periodStart,
                    CallCount   = 1,
                    UpdatedAt   = now,
                });
            }
            else
            {
                usage.CallCount++;
                usage.UpdatedAt = now;
            }

            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to update usage for API key ID {Id}", apiKeyId);
        }
    }

    private static string ComputeHash(string rawKey)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawKey));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
