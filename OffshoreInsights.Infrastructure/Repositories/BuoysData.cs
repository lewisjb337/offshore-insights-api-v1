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
                .OrderBy(b => b.Mmsi)
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
        logger.LogInformation("Fetching current position for buoy MMSI {Mmsi}", request.Mmsi);

        try
        {
            var buoy = await context.Buoys
                .FromSqlRaw(@"SELECT * FROM ""Buoys"" WHERE ""Mmsi"" = {0}", request.Mmsi)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (buoy is null)
            {
                logger.LogWarning("Buoy with MMSI {Mmsi} not found", request.Mmsi);
                throw new KeyNotFoundException($"Buoy with MMSI {request.Mmsi} not found.");
            }

            buoy.CurrentPosition = await context.BuoyPositionHistory
                .AsNoTracking()
                .Where(h => h.BuoyMmsi == request.Mmsi)
                .OrderByDescending(h => h.ReceivedAt)
                .FirstOrDefaultAsync(cancellationToken);

            logger.LogInformation("Successfully fetched position for buoy MMSI {Mmsi}", request.Mmsi);

            return buoy;
        }
        catch (Exception ex) when (ex is not KeyNotFoundException)
        {
            logger.LogError(ex, "Error fetching position for buoy MMSI {Mmsi}", request.Mmsi);
            throw;
        }
    }

    public async Task<PagedResponse<BuoyPositionHistory>> GetBuoyTrackByIdAsync(GetBuoyTrackByIdRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
            "Fetching track for buoy MMSI {Mmsi} (From: {From}, To: {To}, Page: {Page})",
            request.Mmsi, request.From, request.To, request.Page);

        try
        {
            var exists = await context.Buoys
                .AsNoTracking()
                .AnyAsync(b => b.Mmsi == request.Mmsi, cancellationToken);

            if (!exists)
            {
                logger.LogWarning("Buoy with MMSI {Mmsi} not found", request.Mmsi);
                throw new KeyNotFoundException($"Buoy with MMSI {request.Mmsi} not found.");
            }

            var query = context.BuoyPositionHistory
                .AsNoTracking()
                .Where(h => h.BuoyMmsi == request.Mmsi)
                .AsQueryable();

            if (request.From.HasValue)
                query = query.Where(h => h.ReceivedAt >= request.From);

            if (request.To.HasValue)
                query = query.Where(h => h.ReceivedAt <= request.To);

            var result = await query
                .OrderByDescending(h => h.ReceivedAt)
                .ToPagedResponseAsync(request.Page, request.PageSize, cancellationToken);

            logger.LogInformation(
                "Successfully fetched {Count} track points for buoy MMSI {Mmsi} (page {Page} of {TotalPages}, {TotalCount} total)",
                result.Items.Count(), request.Mmsi, result.Page, result.TotalPages, result.TotalCount);

            return result;
        }
        catch (Exception ex) when (ex is not KeyNotFoundException)
        {
            logger.LogError(ex, "Error fetching track for buoy MMSI {Mmsi}", request.Mmsi);
            throw;
        }
    }
}
