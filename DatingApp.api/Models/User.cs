using System;
using System.Collections;
using System.Collections.Generic;

namespace DatingApp.api.Models
{
    public class User
    {
        public int id { get; set; }

        public string UserName { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] Passwordsalt { get; set; }

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
        public ICollection<Photo> Photos { get; set; }
    }
}