using System.ComponentModel.DataAnnotations;

namespace SistemaContable.API.Dtos.Auth
{
    public class RefreshTokenDto
    {
        [Required]
        public string token {  get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
