using System;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.api.DTOS
{
    public class UserForRegisterDTO
    {
        [Required]
        public string UserName {get; set;}

        [Required]
        [StringLength(8,MinimumLength=4,ErrorMessage="minimum length is 4, maximun is 8")]
        public string Password {get; set;}

        [Required]
        public string Gender { get; set; }
        [Required]
        public string KnownAs { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }

        public UserForRegisterDTO()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;            
        }

    }
}