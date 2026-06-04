using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OffshoreInsights.Domain.Entities;
using OffshoreInsights.Persistence.Mapping;

namespace OffshoreInsights.Persistence.Configurations;

public class ApiKeyConfiguration : ExternalEntityConfiguration<ApiKey>
{
    protected override void PostConfigure(EntityTypeBuilder<ApiKey> builder)
    {
        builder.ToTable("ApiKeys");
        builder.HasKey(x => x.Id);

        // AspNetUsers.Id is stored as uuid in PostgreSQL. Npgsql 8+ cannot read uuid as string
        // directly, so we round-trip through Guid: EF asks Npgsql for a Guid (which it handles
        // natively for uuid columns) and the converter then produces the string the entity expects.
        builder.Property(x => x.UserId)
            .IsRequired()
            .HasConversion(
                v => Guid.Parse(v),        // string  → Guid  (writes / parameters)
                v => v.ToString())         // Guid    → string (reads / materialisation)
            .HasColumnType("uuid");
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.KeyHash).IsRequired().HasMaxLength(64);
        builder.Property(x => x.KeyPrefix).IsRequired().HasMaxLength(8);

        // Fast lookup by hash on every authenticated request
        builder.HasIndex(x => x.KeyHash)
            .IsUnique()
            .HasDatabaseName("idx_apikeys_keyhash");

        builder.HasIndex(x => x.UserId)
            .HasDatabaseName("idx_apikeys_userid");
    }
}
