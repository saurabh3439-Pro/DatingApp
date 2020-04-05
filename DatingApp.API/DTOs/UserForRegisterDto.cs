using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTOs
{
    public class UserForRegisterDto
    {
        [Required]
        public string userName { get; set; }
        [Required]
        [StringLength(15, MinimumLength=4, ErrorMessage="Passwrod Must be between 4 and 15 charachters")]
        public string password { get; set; }
    }
}