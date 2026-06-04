using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OffshoreInsights.Application.Features.Buoys.Abstractions;
using OffshoreInsights.Application.Features.Buoys.Requests;
using OffshoreInsights.Domain.Entities;
using OffshoreInsights.Domain.Shared;
using OffshoreInsights.Infrastructure.Shared;
using OffshoreInsights.Persistence.Contexts;

namespace OffshoreInsights.Infrastructure.Repositories;

public class BuoysData(ILogger<BuoysData> logger, ApplicationDbContext context) : IBuoysData
{
    public async Task<PagedResponse<Buoy>> GetBuoysAsync(GetBuoysRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching buoys (Page: {Page})", request.Page);

        try
        {
            var result = await context.Buoys
                .AsNoTracking()
                .OrderBy(b => b.Id)
                .ToPagedResponseAsync(request.Page, request.PageSize, cancellationToken);

            logger.LogInformation(
                "Successfully fetched {Count} buoys (page {Page} of {TotalPages}, {TotalCount} total)",
                result.Items.Count(), result.Page, result.TotalPages, result.TotalCount);

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching buoys");
            throw;
        }
    }

    public async Task<Buoy> GetBuoyPositionByIdAsync(GetBuoyPositionByIdRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching current position for buoy with ID {Id}", request.Id);

        try
        {
            var buoy = await context.Buoys
                .Include(b => b.CurrentPosition)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

            if (buoy is null)
            {
                logger.LogWarning("Buoy with ID {Id} not found", request.Id);
                throw new KeyNotFoundException($"Buoy with ID {request.Id} not found.");
            }

            logger.LogInformation("Successfully fetched position for buoy with ID {Id}", request.Id);

            return buoy;
        }
        catch (Exception ex) when (ex is not KeyNotFoundException)
        {
            logger.LogError(ex, "Error fetching position for buoy with ID {Id}", request.Id);
            throw;
        }
    }

    public async Task<PagedResponse<BuoyPositionHistory>> GetBuoyTrackByIdAsync(GetBuoyTrackByIdRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
            "Fetching track for buoy with ID {Id} (From: {From}, To: {To}, Page: {Page})",
            request.Id, request.From, request.To, request.Page);

        try
        {
            var buoyExists = await context.Buoys
                .AsNoTracking()
                .AnyAsync(b => b.Id == request.Id, cancellationToken);

            if (!buoyExists)
            {
                logger.LogWarning("Buoy with ID {Id} not found", request.Id);
                throw new KeyNotFoundException($"Buoy with ID {request.Id} not found.");
            }

            var query = context.BuoyPositionHistory
                .AsNoTracking()
                .Where(h => h.BuoyId == request.Id)
                .AsQueryable();

            if (request.From.HasValue)
                query = query.Where(h => h.PositionTimestamp >= request.From);

            if (request.To.HasValue)
                query = query.Where(h => h.PositionTimestamp <= request.To);

            var result = await query
                .OrderByDescending(h => h.PositionTimestamp)
                .ToPagedResponseAsync(request.Page, request.PageSize, cancellationToken);

            logger.LogInformation(
                "Successfully fetched {Count} track points for buoy with ID {Id} (page {Page} of {TotalPages}, {TotalCount} total)",
                result.Items.Count(), request.Id, result.Page, result.TotalPages, result.TotalCount);

            return result;
        }
        catch (Exception ex) when (ex is not KeyNotFoundException)
        {
            logger.LogError(ex, "Error fetching track for buoy with ID {Id}", request.Id);
            throw;
        }
    }
}
