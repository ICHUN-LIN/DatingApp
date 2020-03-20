using System.ComponentModel.DataAnnotations;

namespace DatingApp.api.DTOS
{
    public class UserForRegisterDTO
    {
        [Required]
        public string UserName {get; set;}

        [Required]
        [StringLength(8,MinimumLength=4,ErrorMessage="error msg")]
        public string Password {get; set;}
    }
}