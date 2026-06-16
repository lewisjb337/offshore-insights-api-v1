using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OffshoreInsights.Application.Features.Webhooks.Abstractions;
using OffshoreInsights.Application.Features.Webhooks.Requests;
using OffshoreInsights.Domain.Entities;
using OffshoreInsights.Domain.Shared;
using OffshoreInsights.Infrastructure.Shared;
using OffshoreInsights.Persistence.Contexts;

namespace OffshoreInsights.Infrastructure.Repositories;

public class WebhooksData(ILogger<WebhooksData> logger, ApplicationDbContext context) : IWebhooksData
{
    public async Task<PagedResponse<Webhook>> GetWebhooksAsync(GetWebhooksRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching webhooks for user {UserId}", request.UserId);

        try
        {
            var userId = Guid.Parse(request.UserId);

            var result = await context.Webhooks
                .AsNoTracking()
                .Where(w => w.UserId == userId)
                .OrderBy(w => w.CreatedAt)
                .ToPagedResponseAsync(request.Page, request.PageSize, cancellationToken);

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching webhooks for user {UserId}", request.UserId);
            throw;
        }
    }

    public async Task<Webhook> RegisterWebhookAsync(RegisterWebhookRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Registering webhook '{Url}' for user {UserId}", request.Url, request.UserId);

        try
        {
            var userId = Guid.Parse(request.UserId);
            var now    = DateTimeOffset.UtcNow;

            var webhook = new Webhook
            {
                UserId    = userId,
                Url       = request.Url,
                Secret    = request.Secret,
                Events    = request.Events,
                IsActive  = true,
                CreatedAt = now,
                UpdatedAt = now,
            };

            context.Webhooks.Add(webhook);
            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Successfully registered webhook {Id}", webhook.Id);

            return webhook;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error registering webhook '{Url}'", request.Url);
            throw;
        }
    }

    public async Task DeleteWebhookAsync(DeleteWebhookRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Deleting webhook {Id} for user {UserId}", request.Id, request.UserId);

        try
        {
            var userId  = Guid.Parse(request.UserId);
            var webhook = await context.Webhooks
                .FirstOrDefaultAsync(w => w.Id == request.Id && w.UserId == userId, cancellationToken);

            if (webhook is null)
            {
                logger.LogWarning("Webhook {Id} not found for user {UserId}", request.Id, request.UserId);
                throw new KeyNotFoundException($"Webhook {request.Id} not found.");
            }

            context.Webhooks.Remove(webhook);
            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Successfully deleted webhook {Id}", request.Id);
        }
        catch (Exception ex) when (ex is not KeyNotFoundException)
        {
            logger.LogError(ex, "Error deleting webhook {Id}", request.Id);
            throw;
        }
    }
}
