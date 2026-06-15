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

        // Resolve the caller's subscription plan
        var sub = await context.AccountSubscriptions
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.UserId == apiKey.UserId, cancellationToken);

        var plan = Enum.TryParse<AccountPlan>(sub?.Plan ?? "Free", out var p) ? p : AccountPlan.Free;

        // For Free-tier: atomically increment the call counter and check the limit
        // in a single SQL statement to eliminate the read-then-write race condition.
        if (plan == AccountPlan.Free)
        {
            var periodStart = new DateOnly(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            var periodStr   = periodStart.ToString("yyyy-MM-dd");

            var newCount = await context.Database
                .SqlQueryRaw<int>("""
                    INSERT INTO "ApiCallUsage" ("UserId", "PeriodStart", "CallCount", "UpdatedAt")
                    VALUES ({0}, {1}, 1, NOW())
                    ON CONFLICT ("UserId", "PeriodStart")
                    DO UPDATE SET "CallCount" = "ApiCallUsage"."CallCount" + 1, "UpdatedAt" = NOW()
                    RETURNING "CallCount"
                    """, apiKey.UserId, periodStr)
                .FirstAsync(cancellationToken);

            if (newCount > FreePlanCallLimit)
            {
                logger.LogInformation("Free-tier rate limit reached for user {UserId}", apiKey.UserId);
                return new ApiKeyValidationResult(true, apiKey.UserId, AccountPlan.Free, IsRateLimited: true);
            }

            // Usage already incremented above — only update LastUsedAt
            _ = UpdateLastUsedAtAsync(apiKey.Id);
            return new ApiKeyValidationResult(true, apiKey.UserId, plan);
        }

        // Paid plans: fire-and-forget LastUsedAt and usage increment
        _ = UpdateLastUsedAndIncrementUsageAsync(apiKey.Id, apiKey.UserId);

        return new ApiKeyValidationResult(true, apiKey.UserId, plan);
    }

    private async Task UpdateLastUsedAtAsync(long apiKeyId)
    {
        try
        {
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

    private async Task UpdateLastUsedAndIncrementUsageAsync(long apiKeyId, string userId)
    {
        try
        {
            await using var db = await contextFactory.CreateDbContextAsync();

            await db.ApiKeys
                .Where(k => k.Id == apiKeyId)
                .ExecuteUpdateAsync(s => s.SetProperty(k => k.LastUsedAt, DateTime.UtcNow));

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
