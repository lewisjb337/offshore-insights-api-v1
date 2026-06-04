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
        logger.LogInformation("Fetching geofences (Page: {Page})", request.Page);

        try
        {
            var result = await context.Geofences
                .AsNoTracking()
                .OrderBy(g => g.Id)
                .ToPagedResponseAsync(request.Page, request.PageSize, cancellationToken);

            logger.LogInformation(
                "Successfully fetched {Count} geofences (page {Page} of {TotalPages}, {TotalCount} total)",
                result.Items.Count(), result.Page, result.TotalPages, result.TotalCount);

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching geofences");
            throw;
        }
    }

    public async Task<Geofence> CreateGeofenceAsync(CreateGeofenceRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Creating geofence '{Name}' (Type: {Type})", request.Name, request.Type);

        try
        {
            var geofence = new Geofence
            {
                Name            = request.Name,
                Type            = request.Type,
                CenterLatitude  = request.CenterLatitude,
                CenterLongitude = request.CenterLongitude,
                RadiusMetres    = request.RadiusMetres,
                CoordinatesJson = request.Coordinates is not null
                    ? JsonSerializer.Serialize(request.Coordinates)
                    : null,
                TargetMmsisJson = request.TargetMmsis is not null
                    ? JsonSerializer.Serialize(request.TargetMmsis)
                    : null,
                CreatedAt = DateTimeOffset.UtcNow
            };

            context.Geofences.Add(geofence);
            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Successfully created geofence with ID {Id}", geofence.Id);

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
        logger.LogInformation("Deleting geofence with ID {Id}", request.Id);

        try
        {
            var geofence = await context.Geofences
                .FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

            if (geofence is null)
            {
                logger.LogWarning("Geofence with ID {Id} not found", request.Id);
                throw new KeyNotFoundException($"Geofence with ID {request.Id} not found.");
            }

            context.Geofences.Remove(geofence);
            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Successfully deleted geofence with ID {Id}", request.Id);
        }
        catch (Exception ex) when (ex is not KeyNotFoundException)
        {
            logger.LogError(ex, "Error deleting geofence with ID {Id}", request.Id);
            throw;
        }
    }

    public async Task<PagedResponse<GeofenceEvent>> GetGeofenceEventsAsync(GetGeofenceEventsRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
            "Fetching events for geofence with ID {Id} (From: {From}, To: {To}, Page: {Page})",
            request.Id, request.From, request.To, request.Page);

        try
        {
            var geofenceExists = await context.Geofences
                .AsNoTracking()
                .AnyAsync(g => g.Id == request.Id, cancellationToken);

            if (!geofenceExists)
            {
                logger.LogWarning("Geofence with ID {Id} not found", request.Id);
                throw new KeyNotFoundException($"Geofence with ID {request.Id} not found.");
            }

            var query = context.GeofenceEvents
                .AsNoTracking()
                .Where(e => e.GeofenceId == request.Id)
                .AsQueryable();

            if (request.From.HasValue)
                query = query.Where(e => e.OccurredAt >= request.From);

            if (request.To.HasValue)
                query = query.Where(e => e.OccurredAt <= request.To);

            var result = await query
                .OrderByDescending(e => e.OccurredAt)
                .ToPagedResponseAsync(request.Page, request.PageSize, cancellationToken);

            logger.LogInformation(
                "Successfully fetched {Count} events for geofence with ID {Id} (page {Page} of {TotalPages}, {TotalCount} total)",
                result.Items.Count(), request.Id, result.Page, result.TotalPages, result.TotalCount);

            return result;
        }
        catch (Exception ex) when (ex is not KeyNotFoundException)
        {
            logger.LogError(ex, "Error fetching events for geofence with ID {Id}", request.Id);
            throw;
        }
    }
}
