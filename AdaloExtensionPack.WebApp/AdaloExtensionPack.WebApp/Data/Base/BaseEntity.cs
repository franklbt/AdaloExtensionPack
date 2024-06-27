namespace AdaloExtensionPack.WebApp.Data;

public class BaseEntity<TKey> : IBaseEntity<TKey>
{
    public TKey Id { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset? LatestModificationDate { get; set; }
    public bool IsDeleted { get; set; }
}
