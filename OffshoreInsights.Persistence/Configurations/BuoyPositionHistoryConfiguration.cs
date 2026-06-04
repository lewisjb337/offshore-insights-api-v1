using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OffshoreInsights.Domain.Entities;
using OffshoreInsights.Persistence.Mapping;

namespace OffshoreInsights.Persistence.Configurations;

public class BuoyPositionHistoryConfiguration : ExternalEntityConfiguration<BuoyPositionHistory>
{
    protected override void PostConfigure(EntityTypeBuilder<BuoyPositionHistory> builder)
    {
        builder.ToTable("BuoyPositionHistory");
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.BuoyId, x.PositionTimestamp })
            .IsDescending(false, true)
            .HasDatabaseName("idx_buoy_position_history_buoy_timestamp");
    }
}
