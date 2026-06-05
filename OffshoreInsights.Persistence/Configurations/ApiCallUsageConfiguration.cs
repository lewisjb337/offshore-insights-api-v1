using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OffshoreInsights.Domain.Entities;
using OffshoreInsights.Persistence.Mapping;

namespace OffshoreInsights.Persistence.Configurations;

public class ApiCallUsageConfiguration : ExternalEntityConfiguration<ApiCallUsage>
{
    protected override void PostConfigure(EntityTypeBuilder<ApiCallUsage> builder)
    {
        builder.ToTable("ApiCallUsage");
        builder.HasKey(x => x.Id);

        // Same uuid↔string round-trip as ApiKeyConfiguration
        builder.Property(x => x.UserId)
            .IsRequired()
            .HasConversion(
                v => Guid.Parse(v),
                v => v.ToString())
            .HasColumnType("uuid");

        builder.Property(x => x.PeriodStart).IsRequired();
        builder.Property(x => x.CallCount).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();

        // Unique constraint so we can use ON CONFLICT for upsert
        builder.HasIndex(x => new { x.UserId, x.PeriodStart })
            .IsUnique()
            .HasDatabaseName("idx_apiusage_user_period");
    }
}
