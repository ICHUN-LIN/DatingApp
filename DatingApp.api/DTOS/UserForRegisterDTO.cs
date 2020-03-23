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
    }
}