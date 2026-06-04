using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OffshoreInsights.Domain.Entities;
using OffshoreInsights.Persistence.Mapping;

namespace OffshoreInsights.Persistence.Configurations;

public class GeofenceConfiguration : ExternalEntityConfiguration<Geofence>
{
    protected override void PostConfigure(EntityTypeBuilder<Geofence> builder)
    {
        builder.ToTable("Geofences");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Type)
            .HasConversion<string>();

        builder.Property(x => x.CoordinatesJson)
            .HasColumnType("jsonb");

        builder.Property(x => x.TargetMmsisJson)
            .HasColumnType("jsonb");
    }
}
