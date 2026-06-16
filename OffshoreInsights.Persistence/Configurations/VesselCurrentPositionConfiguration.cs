using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OffshoreInsights.Domain.Entities;
using OffshoreInsights.Domain.Enums;
using OffshoreInsights.Persistence.Mapping;

namespace OffshoreInsights.Persistence.Configurations;

public class VesselCurrentPositionConfiguration : ExternalEntityConfiguration<VesselCurrentPosition>
{
    protected override void PostConfigure(EntityTypeBuilder<VesselCurrentPosition> builder)
    {
        builder.ToTable("VesselCurrentPosition");
        builder.HasKey(x => x.Id);
    
        builder.Property(x => x.NavStatus)
            .HasConversion(
                v => v.HasValue ? (long)(int)v.Value : (long?)null,
                v => v.HasValue ? (NavStatus)(int)v.Value : (NavStatus?)null)
            .HasColumnType("bigint");
    }
}