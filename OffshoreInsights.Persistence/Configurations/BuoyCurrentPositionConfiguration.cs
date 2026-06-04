using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OffshoreInsights.Domain.Entities;
using OffshoreInsights.Persistence.Mapping;

namespace OffshoreInsights.Persistence.Configurations;

public class BuoyCurrentPositionConfiguration : ExternalEntityConfiguration<BuoyCurrentPosition>
{
    protected override void PostConfigure(EntityTypeBuilder<BuoyCurrentPosition> builder)
    {
        builder.ToTable("BuoyCurrentPosition");
        builder.HasKey(x => x.Id);
    }
}
