using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OffshoreInsights.Domain.Entities;
using OffshoreInsights.Persistence.Mapping;

namespace OffshoreInsights.Persistence.Configurations;

public class VesselPositionHistoryConfiguration : ExternalEntityConfiguration<VesselPositionHistory>
{
    protected override void PostConfigure(EntityTypeBuilder<VesselPositionHistory> builder)
    {
        builder.ToTable("VesselPositionHistory");
        builder.HasKey(x => x.Id);
    
        builder.Property(x => x.NavStatus)
            .HasConversion<int>();
    
        builder.HasIndex(x => new { x.VesselId, x.PositionTimestamp })
            .IsDescending(false, true)
            .HasDatabaseName("idx_vessel_position_history_vessel_timestamp");

        builder.HasIndex(x => new { x.PositionTimestamp, x.Latitude, x.Longitude })
            .IsDescending(true, false, false)
            .HasDatabaseName("idx_vph_timestamp_lat_lon");
    }
}