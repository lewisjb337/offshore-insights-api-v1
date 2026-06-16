using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OffshoreInsights.Application.Features.Geofences.Abstractions;
using OffshoreInsights.Application.Features.Geofences.Requests;
using OffshoreInsights.Domain.Entities;
using OffshoreInsights.Domain.Shared;
using OffshoreInsights.Infrastructure.Shared;
using OffshoreInsights.Persistence.Contexts;

namespace OffshoreInsights.Infrastructure.Repositories;

public class GeofencesData(ILogger<GeofencesData> logger, ApplicationDbContext context) : IGeofencesData
{
    public async Task<PagedResponse<Geofence>> GetGeofencesAsync(GetGeofencesRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching geofences for user {UserId} (Page: {Page})", request.UserId, request.Page);

        try
        {
            var userId = Guid.Parse(request.UserId);

            var result = await context.Geofences
                .AsNoTracking()
                .Where(g => g.UserId == userId)
                .OrderBy(g => g.CreatedAt)
                .ToPagedResponseAsync(request.Page, request.PageSize, cancellationToken);

            logger.LogInformation(
                "Successfully fetched {Count} geofences (page {Page} of {TotalPages}, {TotalCount} total)",
                result.Items.Count(), result.Page, result.TotalPages, result.TotalCount);

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching geofences for user {UserId}", request.UserId);
            throw;
        }
    }

    public async Task<Geofence> CreateGeofenceAsync(CreateGeofenceRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Creating geofence '{Name}' for user {UserId}", request.Name, request.UserId);

        try
        {
            var userId = Guid.Parse(request.UserId);
            var now    = DateTimeOffset.UtcNow;

            string geometry;
            string? circleCenter = null;

            if (request.ShapeType == "circle" && request.CircleCenterLat.HasValue && request.CircleCenterLon.HasValue)
            {
                // Store circle center as a GeoJSON point for consistency
                circleCenter = JsonSerializer.Serialize(new { lat = request.CircleCenterLat, lon = request.CircleCenterLon });
                geometry     = circleCenter;
            }
            else
            {
                geometry = request.Geometry ?? "{}";
            }

            var geofence = new Geofence
            {
                UserId               = userId,
                Name                 = request.Name,
                ShapeType            = request.ShapeType,
                Colour               = request.Colour ?? "#0284c7",
                Geometry             = geometry,
                CircleCenter         = circleCenter,
                CircleRadiusKm       = request.CircleRadiusKm,
                VesselTypeFilter     = request.VesselTypeFilter,
                NotificationsEnabled = request.NotificationsEnabled,
                WatchedBuoyMmsi      = request.WatchedBuoyMmsi,
                CreatedAt            = now,
                UpdatedAt            = now,
            };

            context.Geofences.Add(geofence);
            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Successfully created geofence {Id}", geofence.Id);

            return geofence;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating geofence '{Name}'", request.Name);
            throw;
        }
    }

    public async Task DeleteGeofenceAsync(DeleteGeofenceRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Deleting geofence {Id} for user {UserId}", request.Id, request.UserId);

        try
        {
            var userId   = Guid.Parse(request.UserId);
            var geofence = await context.Geofences
                .FirstOrDefaultAsync(g => g.Id == request.Id && g.UserId == userId, cancellationToken);

            if (geofence is null)
            {
                logger.LogWarning("Geofence {Id} not found for user {UserId}", request.Id, request.UserId);
                throw new KeyNotFoundException($"Geofence {request.Id} not found.");
            }

            context.Geofences.Remove(geofence);
            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Successfully deleted geofence {Id}", request.Id);
        }
        catch (Exception ex) when (ex is not KeyNotFoundException)
        {
            logger.LogError(ex, "Error deleting geofence {Id}", request.Id);
            throw;
        }
    }

    public async Task<PagedResponse<GeofenceEvent>> GetGeofenceEventsAsync(GetGeofenceEventsRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
            "Fetching events for geofence {Id} (From: {From}, To: {To}, Page: {Page})",
            request.Id, request.From, request.To, request.Page);

        try
        {
            var userId = Guid.Parse(request.UserId);

            var geofenceExists = await context.Geofences
                .AsNoTracking()
                .AnyAsync(g => g.Id == request.Id && g.UserId == userId, cancellationToken);

            if (!geofenceExists)
            {
                logger.LogWarning("Geofence {Id} not found for user {UserId}", request.Id, request.UserId);
                throw new KeyNotFoundException($"Geofence {request.Id} not found.");
            }

            var query = context.GeofenceEvents
                .AsNoTracking()
                .Where(e => e.GeofenceId == request.Id && e.UserId == userId)
                .AsQueryable();

            if (request.From.HasValue)
                query = query.Where(e => e.CreatedAt >= request.From);

            if (request.To.HasValue)
                query = query.Where(e => e.CreatedAt <= request.To);

            var result = await query
                .OrderByDescending(e => e.CreatedAt)
                .ToPagedResponseAsync(request.Page, request.PageSize, cancellationToken);

            logger.LogInformation(
                "Successfully fetched {Count} events for geofence {Id}",
                result.Items.Count(), request.Id);

            return result;
        }
        catch (Exception ex) when (ex is not KeyNotFoundException)
        {
            logger.LogError(ex, "Error fetching events for geofence {Id}", request.Id);
            throw;
        }
    }
}
