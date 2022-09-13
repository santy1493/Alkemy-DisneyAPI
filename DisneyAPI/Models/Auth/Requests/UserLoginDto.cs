using System.ComponentModel.DataAnnotations;

namespace DisneyAPI.Models.Auth.Requests
{
    public class UserLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
