namespace OffshoreInsights.Domain.Shared;

public abstract class BaseEntity
{
    public long Id { get; set; }
    public Guid Key { get; set; } = Guid.NewGuid();
    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset? DateModified { get; set; }
    public Guid? CreatedById { get; set; }
    public Guid? ModifiedById { get; set; }
}