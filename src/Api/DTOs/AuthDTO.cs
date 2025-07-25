using System.ComponentModel.DataAnnotations;

namespace Api.DTOs
{
    public class AuthDTO
    {
        [Required]
        public string UserName = string.Empty;

        [Required]
        public string Password = string.Empty;
    }
}
