using System.ComponentModel.DataAnnotations;

namespace CORE.DTOs.Auth
{
    public class RefreshTokenDto
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
