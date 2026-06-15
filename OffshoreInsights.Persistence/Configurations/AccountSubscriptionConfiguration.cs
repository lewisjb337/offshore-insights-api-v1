using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OffshoreInsights.Domain.Entities;
using OffshoreInsights.Persistence.Mapping;

namespace OffshoreInsights.Persistence.Configurations;

public class AccountSubscriptionConfiguration : ExternalEntityConfiguration<AccountSubscription>
{
    protected override void PostConfigure(EntityTypeBuilder<AccountSubscription> builder)
    {
        builder.ToTable("AccountSubscriptions");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .IsRequired()
            .HasConversion(
                v => Guid.Parse(v),
                v => v.ToString())
            .HasColumnType("uuid");

        builder.Property(x => x.Plan).IsRequired().HasMaxLength(20);
        builder.Property(x => x.Status).IsRequired().HasMaxLength(20);

        builder.HasIndex(x => x.UserId)
            .IsUnique()
            .HasDatabaseName("idx_accountsubs_userid");
    }
}
