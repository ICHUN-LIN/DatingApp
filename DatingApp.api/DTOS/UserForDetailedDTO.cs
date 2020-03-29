using System;
using System.Collections.Generic;
using DatingApp.api.Models;

namespace DatingApp.api.DTOS
{
    public class UserForDetailedDTO
    {
        public int id { get; set; }

        public string UserName { get; set; }

        public string Gender { get; set; }

        public int Age { get; set; }

        public string Knownas { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastActive { get; set; }

        public string Introduction { get; set; }

        public string LookingFor { get; set; }

        public string Interests { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string PhotoUrl { get; set; }

        public ICollection<PhotoDetailedDto> Photos {get; set; }
    }
}