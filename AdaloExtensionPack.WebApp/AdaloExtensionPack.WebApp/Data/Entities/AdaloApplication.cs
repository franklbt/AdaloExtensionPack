namespace AdaloExtensionPack.WebApp.Data;

public class AdaloApplication : BaseEntity<int>
{
    public string Name { get; set; }
    public string UserId { get; set; }
    public virtual User? User { get; set; }
    public Guid AdaloId { get; set; }

    public virtual ICollection<AdaloApplicationTable> Tables { get; set; }
}
