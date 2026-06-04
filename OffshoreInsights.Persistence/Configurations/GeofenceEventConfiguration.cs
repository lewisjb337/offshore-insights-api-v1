using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OffshoreInsights.Domain.Entities;
using OffshoreInsights.Persistence.Mapping;

namespace OffshoreInsights.Persistence.Configurations;

public class GeofenceEventConfiguration : ExternalEntityConfiguration<GeofenceEvent>
{
    protected override void PostConfigure(EntityTypeBuilder<GeofenceEvent> builder)
    {
        builder.ToTable("GeofenceEvents");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.EventType)
            .HasConversion<string>();

        builder.HasOne(x => x.Geofence)
            .WithMany(x => x.Events)
            .HasForeignKey(x => x.GeofenceId);

        builder.HasIndex(x => new { x.GeofenceId, x.OccurredAt })
            .IsDescending(false, true)
            .HasDatabaseName("idx_geofence_events_geofence_occurred");
    }
}
