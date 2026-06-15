using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OffshoreInsights.Application.Features.ApiKeys.Abstractions;
using OffshoreInsights.Application.Features.ApiKeys.Models;
using OffshoreInsights.Domain.Entities;
using OffshoreInsights.Domain.Enums;
using OffshoreInsights.Persistence.Contexts;

namespace OffshoreInsights.Infrastructure.Services;

public class ApiKeyValidator(
    ILogger<ApiKeyValidator> logger,
    ApplicationDbContext context,
    IDbContextFactory<ApplicationDbContext> contextFactory) : IApiKeyValidator
{
    private const int FreePlanCallLimit = 100;

    public async Task<ApiKeyValidationResult> ValidateAsync(string rawKey, CancellationToken cancellationToken = default)
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
            return new ApiKeyValidationResult(false);
        }

        var sub = await context.AccountSubscriptions
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.UserId == apiKey.UserId, cancellationToken);

        var plan = Enum.TryParse<AccountPlan>(sub?.Plan ?? "Free", out var p) ? p : AccountPlan.Free;

        // Free-tier: check the current call count without incrementing.
        // The increment happens in RecordUsageAsync, called by the filter only after a 2xx response.
        if (plan == AccountPlan.Free)
        {
            var periodStart = new DateOnly(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            var usage = await context.ApiCallUsage
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == apiKey.UserId && u.PeriodStart == periodStart, cancellationToken);

            if ((usage?.CallCount ?? 0) >= FreePlanCallLimit)
            {
                logger.LogInformation("Free-tier rate limit reached for user {UserId}", apiKey.UserId);
                return new ApiKeyValidationResult(true, apiKey.UserId, AccountPlan.Free, IsRateLimited: true, ApiKeyId: apiKey.Id);
            }
        }

        return new ApiKeyValidationResult(true, apiKey.UserId, plan, ApiKeyId: apiKey.Id);
    }

    /// <inheritdoc/>
    public async Task RecordUsageAsync(string userId, long apiKeyId)
    {
        try
        {
            await using var db = await contextFactory.CreateDbContextAsync();

            await db.ApiKeys
                .Where(k => k.Id == apiKeyId)
                .ExecuteUpdateAsync(s => s.SetProperty(k => k.LastUsedAt, DateTime.UtcNow));

            var periodStart = new DateOnly(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            var periodStr   = periodStart.ToString("yyyy-MM-dd");

            // Atomic upsert so concurrent requests don't race past the limit
            await db.Database.ExecuteSqlRawAsync("""
                INSERT INTO "ApiCallUsage" ("UserId", "PeriodStart", "CallCount", "UpdatedAt")
                VALUES ({0}, {1}, 1, NOW())
                ON CONFLICT ("UserId", "PeriodStart")
                DO UPDATE SET "CallCount" = "ApiCallUsage"."CallCount" + 1, "UpdatedAt" = NOW()
                """, userId, periodStr);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to record usage for API key ID {Id}", apiKeyId);
        }
    }

    private static string ComputeHash(string rawKey)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawKey));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
