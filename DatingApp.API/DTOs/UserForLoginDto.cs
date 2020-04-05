using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTOs
{
    public class UserForLoginDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string password { get; set; }
    }
}