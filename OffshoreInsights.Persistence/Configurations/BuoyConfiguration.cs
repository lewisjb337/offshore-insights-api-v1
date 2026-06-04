using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OffshoreInsights.Domain.Entities;
using OffshoreInsights.Persistence.Mapping;

namespace OffshoreInsights.Persistence.Configurations;

public class BuoyConfiguration : ExternalEntityConfiguration<Buoy>
{
    protected override void PostConfigure(EntityTypeBuilder<Buoy> builder)
    {
        builder.ToTable("Buoys");
        builder.HasKey(x => x.Id);
    }
}
