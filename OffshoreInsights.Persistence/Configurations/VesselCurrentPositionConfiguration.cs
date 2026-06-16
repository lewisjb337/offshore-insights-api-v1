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

        // NavStatus is stored as bigint; HasConversion<long> maps the enum via its numeric value.
        builder.Property(x => x.NavStatus)
            .HasConversion<long>()
            .HasColumnType("bigint");
    }
}