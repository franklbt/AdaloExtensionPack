using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace AdaloExtensionPack.WebApp.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class User : IdentityUser, IBaseEntity<string>
{
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset? LatestModificationDate { get; set; }
    public bool IsDeleted { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    [NotMapped]
    public string FullName => this switch
    {
        { FirstName.Length: > 0, LastName.Length: > 0 } => $"{FirstName} {LastName}",
        { FirstName.Length: > 0 } => FirstName,
        { LastName.Length: > 0 } => LastName,
        _ => ""
    };

    public virtual ICollection<AdaloApplication> Applications { get; set; }
}
