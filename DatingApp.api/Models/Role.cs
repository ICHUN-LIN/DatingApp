using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DatingApp.api.Models
{
    public class Role: IdentityRole<int>
    {
        virtual public ICollection<UserRole> UserRoles { get; set;}
    }
}