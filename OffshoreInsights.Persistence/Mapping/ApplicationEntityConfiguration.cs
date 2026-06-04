using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OffshoreInsights.Domain.Shared;

namespace OffshoreInsights.Persistence.Mapping;

public class ApplicationEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
{
    public void Configure(EntityTypeBuilder<TEntity> builder) => PostConfigure(builder);

    public void ApplyConfiguration(ModelBuilder builder)
    {
        builder.ApplyConfiguration(this);
    }

    protected virtual void PostConfigure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Key)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("NEWID()");
        builder.Property(x => x.DateCreated)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("SYSUTCDATETIME()");
        builder.Property(x => x.DateModified)
            .ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql("SYSUTCDATETIME()");
    }
}