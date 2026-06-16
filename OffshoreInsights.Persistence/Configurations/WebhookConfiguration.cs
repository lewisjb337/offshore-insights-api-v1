using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OffshoreInsights.Domain.Entities;

namespace OffshoreInsights.Persistence.Configurations;

public class WebhookConfiguration : IEntityTypeConfiguration<Webhook>
{
    public void Configure(EntityTypeBuilder<Webhook> builder)
    {
        builder.ToTable("webhooks");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.Id).HasColumnName("id");
        builder.Property(w => w.UserId).HasColumnName("user_id");
        builder.Property(w => w.Url).HasColumnName("url");
        builder.Property(w => w.Secret).HasColumnName("secret");
        builder.Property(w => w.Events).HasColumnName("events").HasColumnType("text[]");
        builder.Property(w => w.IsActive).HasColumnName("is_active");
        builder.Property(w => w.CreatedAt).HasColumnName("created_at");
        builder.Property(w => w.UpdatedAt).HasColumnName("updated_at");
    }
}
