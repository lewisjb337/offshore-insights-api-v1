using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OffshoreInsights.Persistence.Mapping;

public abstract class ExternalEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : class
{
    public void Configure(EntityTypeBuilder<TEntity> builder) => PostConfigure(builder);

    protected virtual void PostConfigure(EntityTypeBuilder<TEntity> builder) { }
}