using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OffshoreInsights.Domain.Entities;
using OffshoreInsights.Persistence.Mapping;

namespace OffshoreInsights.Persistence.Configurations;

public class GeofenceEventConfiguration : ExternalEntityConfiguration<GeofenceEvent>
{
    protected override void PostConfigure(EntityTypeBuilder<GeofenceEvent> builder)
    {
        builder.ToTable("geofence_notifications");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("uuid");
        builder.Property(x => x.UserId).HasColumnName("user_id").HasColumnType("uuid");
        builder.Property(x => x.GeofenceId).HasColumnName("geofence_id").HasColumnType("uuid");
        builder.Property(x => x.GeofenceName).HasColumnName("geofence_name");
        builder.Property(x => x.VesselId).HasColumnName("vessel_id");
        builder.Property(x => x.VesselName).HasColumnName("vessel_name");
        builder.Property(x => x.VesselType).HasColumnName("vessel_type");
        builder.Property(x => x.EventType).HasColumnName("event_type");
        builder.Property(x => x.SourceType).HasColumnName("source_type");
        builder.Property(x => x.EnteredAt).HasColumnName("entered_at");
        builder.Property(x => x.ExitedAt).HasColumnName("exited_at");
        builder.Property(x => x.DurationMinutes).HasColumnName("duration_minutes");
        builder.Property(x => x.Read).HasColumnName("read");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        builder.Property(x => x.BuoyName).HasColumnName("buoy_name");
        builder.Property(x => x.BuoyMmsi).HasColumnName("buoy_mmsi");
    }
}
