using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OffshoreInsights.Domain.Entities;
using OffshoreInsights.Persistence.Converters;
using OffshoreInsights.Persistence.Mapping;

namespace OffshoreInsights.Persistence.Configurations;

public class VesselConfiguration : ExternalEntityConfiguration<Vessel>
{
    protected override void PostConfigure(EntityTypeBuilder<Vessel> builder)
    {
        builder.ToTable("Vessels");

        builder.Property(x => x.VesselType)
            .HasConversion(new VesselTypeConverter());
    }
}