using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OffshoreInsights.Application.Features.Webhooks.Abstractions;
using OffshoreInsights.Application.Features.Webhooks.Requests;
using OffshoreInsights.Domain.Entities;
using OffshoreInsights.Domain.Enums;
using OffshoreInsights.Domain.Shared;
using OffshoreInsights.Infrastructure.Shared;
using OffshoreInsights.Persistence.Contexts;

namespace OffshoreInsights.Infrastructure.Repositories;

public class WebhooksData(ILogger<WebhooksData> logger, ApplicationDbContext context) : IWebhooksData
{
    public async Task<PagedResponse<Webhook>> GetWebhooksAsync(GetWebhooksRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching webhooks (Page: {Page})", request.Page);

        try
        {
            var result = await context.Webhooks
                .AsNoTracking()
                .OrderBy(w => w.Id)
                .ToPagedResponseAsync(request.Page, request.PageSize, cancellationToken);

            logger.LogInformation(
                "Successfully fetched {Count} webhooks (page {Page} of {TotalPages}, {TotalCount} total)",
                result.Items.Count(), result.Page, result.TotalPages, result.TotalCount);

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching webhooks");
            throw;
        }
    }

    public async Task<Webhook> RegisterWebhookAsync(RegisterWebhookRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Registering webhook for URL {Url}", request.Url);

        try
        {
            var webhook = new Webhook
            {
                Url            = request.Url,
                Secret         = request.Secret,
                DeliveryStatus = WebhookDeliveryStatus.Active,
                CreatedAt      = DateTimeOffset.UtcNow
            };

            context.Webhooks.Add(webhook);
            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Successfully registered webhook with ID {Id}", webhook.Id);

            return webhook;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error registering webhook for URL {Url}", request.Url);
            throw;
        }
    }

    public async Task DeleteWebhookAsync(DeleteWebhookRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Deleting webhook with ID {Id}", request.Id);

        try
        {
            var webhook = await context.Webhooks
                .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

            if (webhook is null)
            {
                logger.LogWarning("Webhook with ID {Id} not found", request.Id);
                throw new KeyNotFoundException($"Webhook with ID {request.Id} not found.");
            }

            context.Webhooks.Remove(webhook);
            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Successfully deleted webhook with ID {Id}", request.Id);
        }
        catch (Exception ex) when (ex is not KeyNotFoundException)
        {
            logger.LogError(ex, "Error deleting webhook with ID {Id}", request.Id);
            throw;
        }
    }
}
