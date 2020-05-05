using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DatingApp.api.Models
{
    //int force pk is interger
    public class User : IdentityUser<int>
    {
        /* remove because it's already from parent's class
        public int id { get; set; }

        public string UserName { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] Passwordsalt { get; set; }
        */

        public string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Knownas { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastActive { get; set; }

        public string Introduction { get; set; }

        public string LookingFor { get; set; }

        public string Interests { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        //Photo is Navigation Property of User Class 
        public virtual ICollection<Photo> Photos { get; set; }

        public virtual ICollection<Like> Likers { get; set; }
        public virtual ICollection<Like> Likees { get; set; }

        public virtual ICollection<UserRole> UserRoles {get; set;} 
    }
}