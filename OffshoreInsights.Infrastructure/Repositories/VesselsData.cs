using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OffshoreInsights.Application.Features.Vessels.Abstractions;
using OffshoreInsights.Application.Features.Vessels.Requests;
using OffshoreInsights.Domain.Entities;
using OffshoreInsights.Domain.Shared;
using OffshoreInsights.Infrastructure.Shared;
using OffshoreInsights.Persistence.Contexts;

namespace OffshoreInsights.Infrastructure.Repositories;

public class VesselsData(ILogger<VesselsData> logger, ApplicationDbContext context) : IVesselsData
{
    public async Task<Vessel> GetVesselByMmsiAsync(GetVesselByMmsiRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching vessel with MMSI {Mmsi}", request.Mmsi);

        try
        {
            var vessel = await context.Vessels
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Mmsi == request.Mmsi, cancellationToken);

            if (vessel is null)
            {
                logger.LogWarning("Vessel with MMSI {Mmsi} not found", request.Mmsi);
                throw new KeyNotFoundException($"Vessel with MMSI {request.Mmsi} not found.");
            }

            logger.LogInformation("Successfully fetched vessel with MMSI {Mmsi}", request.Mmsi);

            return vessel;
        }
        catch (Exception ex) when (ex is not KeyNotFoundException)
        {
            logger.LogError(ex, "Error fetching vessel with MMSI {Mmsi}", request.Mmsi);
            throw;
        }
    }

    public async Task<PagedResponse<Vessel>> GetVesselsAsync(GetVesselsRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
            "Fetching vessels (MMSIs: {MmsiCount}, Type: {Type}, IsOffshoreWindRelevant: {IsOffshoreWindRelevant}, Page: {Page})",
            request.Mmsis?.Count() ?? 0, request.Type, request.IsOffshoreWindRelevant, request.Page);

        try
        {
            var query = context.Vessels.AsNoTracking().AsQueryable();

            if (request.Mmsis?.Any() == true)
                query = query.Where(v => request.Mmsis.Contains(v.Mmsi));

            if (request.Type.HasValue)
                query = query.Where(v => v.VesselType == request.Type);

            if (request.IsOffshoreWindRelevant.HasValue)
                query = query.Where(v => v.IsOffshoreWindRelevant == request.IsOffshoreWindRelevant);

            var result = await query
                .OrderBy(v => v.Mmsi)
                .ToPagedResponseAsync(request.Page, request.PageSize, cancellationToken);

            logger.LogInformation(
                "Successfully fetched {Count} vessels (page {Page} of {TotalPages}, {TotalCount} total)",
                result.Items.Count(), result.Page, result.TotalPages, result.TotalCount);

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching vessels");
            throw;
        }
    }

    public async Task<PagedResponse<VesselPositionHistory>> GetVesselTrackByMmsiAsync(GetVesselTrackByMmsiRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
            "Fetching track for vessel with MMSI {Mmsi} (From: {From}, To: {To}, Page: {Page})",
            request.Mmsi, request.From, request.To, request.Page);

        try
        {
            var vesselId = await context.Vessels
                .AsNoTracking()
                .Where(v => v.Mmsi == request.Mmsi)
                .Select(v => (long?)v.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (vesselId is null)
            {
                logger.LogWarning("Vessel with MMSI {Mmsi} not found", request.Mmsi);
                throw new KeyNotFoundException($"Vessel with MMSI {request.Mmsi} not found.");
            }

            var query = context.VesselPositionHistory
                .AsNoTracking()
                .Where(h => h.VesselId == vesselId)
                .AsQueryable();

            if (request.From.HasValue)
                query = query.Where(h => h.PositionTimestamp >= request.From);

            if (request.To.HasValue)
                query = query.Where(h => h.PositionTimestamp <= request.To);

            var result = await query
                .OrderByDescending(h => h.PositionTimestamp)
                .ToPagedResponseAsync(request.Page, request.PageSize, cancellationToken);

            logger.LogInformation(
                "Successfully fetched {Count} track points for vessel with MMSI {Mmsi} (page {Page} of {TotalPages}, {TotalCount} total)",
                result.Items.Count(), request.Mmsi, result.Page, result.TotalPages, result.TotalCount);

            return result;
        }
        catch (Exception ex) when (ex is not KeyNotFoundException)
        {
            logger.LogError(ex, "Error fetching track for vessel with MMSI {Mmsi}", request.Mmsi);
            throw;
        }
    }

    public async Task<Vessel> GetVesselPositionByMmsiAsync(GetVesselPositionByMmsiRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching current position for vessel with MMSI {Mmsi}", request.Mmsi);

        try
        {
            var vessel = await context.Vessels
                .FromSqlRaw(@"SELECT * FROM ""Vessels"" WHERE ""Mmsi"" = {0}", request.Mmsi)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (vessel is null)
            {
                logger.LogWarning("Vessel with MMSI {Mmsi} not found", request.Mmsi);
                throw new KeyNotFoundException($"Vessel with MMSI {request.Mmsi} not found.");
            }

            vessel.CurrentPosition = await context.VesselCurrentPositions
                .FromSqlRaw(@"SELECT * FROM ""VesselCurrentPosition"" WHERE ""VesselId"" = {0}", vessel.Id)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            logger.LogInformation("Successfully fetched position for vessel with MMSI {Mmsi}", request.Mmsi);

            return vessel;
        }
        catch (Exception ex) when (ex is not KeyNotFoundException)
        {
            logger.LogError(ex, "Error fetching position for vessel with MMSI {Mmsi}", request.Mmsi);
            throw;
        }
    }

    public async Task<PagedResponse<Vessel>> GetVesselPositionsAsync(GetVesselPositionsRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
            "Fetching vessel positions (MMSIs: {MmsiCount}, Bounds: [{MinLat},{MinLon}] to [{MaxLat},{MaxLon}], Page: {Page})",
            request.Mmsis?.Count() ?? 0, request.MinLatitude, request.MinLongitude, request.MaxLatitude, request.MaxLongitude, request.Page);

        try
        {
            var query = context.Vessels
                .Include(v => v.CurrentPosition)
                .AsNoTracking()
                .AsQueryable();

            if (request.Mmsis?.Any() == true)
                query = query.Where(v => request.Mmsis.Contains(v.Mmsi));

            if (request.MinLatitude.HasValue)
                query = query.Where(v => v.CurrentPosition != null && v.CurrentPosition.Latitude >= request.MinLatitude);

            if (request.MaxLatitude.HasValue)
                query = query.Where(v => v.CurrentPosition != null && v.CurrentPosition.Latitude <= request.MaxLatitude);

            if (request.MinLongitude.HasValue)
                query = query.Where(v => v.CurrentPosition != null && v.CurrentPosition.Longitude >= request.MinLongitude);

            if (request.MaxLongitude.HasValue)
                query = query.Where(v => v.CurrentPosition != null && v.CurrentPosition.Longitude <= request.MaxLongitude);

            var result = await query
                .OrderBy(v => v.Mmsi)
                .ToPagedResponseAsync(request.Page, request.PageSize, cancellationToken);

            logger.LogInformation(
                "Successfully fetched {Count} vessel positions (page {Page} of {TotalPages}, {TotalCount} total)",
                result.Items.Count(), result.Page, result.TotalPages, result.TotalCount);

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching vessel positions");
            throw;
        }
    }
}
