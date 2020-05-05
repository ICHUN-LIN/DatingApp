using Microsoft.AspNetCore.Identity;

namespace DatingApp.api.Models
{
    //Join Table of User and Role : Muti to Muti Relationship
    public class UserRole: IdentityUserRole<int>
    {
       virtual public User User { get; set; }
       virtual public Role Role { get; set; }
    }
}