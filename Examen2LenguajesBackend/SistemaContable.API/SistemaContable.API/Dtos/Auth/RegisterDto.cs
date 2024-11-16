using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.API.Dtos.Auth
{
    public class RegisterDto
    {
        [StringLength(70, MinimumLength = 3)] //Se puede cambiar a nivel de modelo
        [Column("first_name")]
        [Required]
        public string FirstName { get; set; }

        [StringLength(70, MinimumLength = 3)] //Se puede cambiar a nivel de modelo
        [Display(Name ="last_name")]
        [Required]
        public string LastName { get; set; }

        [Display(Name = "Correo Electrónico")]
        [Required(ErrorMessage = "EL campo {0} es requerido.")]
        [EmailAddress(ErrorMessage = "El campo {0} no es valido.")]
        public string Email { get; set; }

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        // 1 mayuscula, 1 minuscula, 1 caracter especial, 1 numero, sea mayor a 8 caracteres
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "La contraseña debe ser segura y contener al menos 8 caracteres, incluyendo minúsculas, mayúsculas, números y caracteres especiales.")]
        public string Password { get; set; }

        [Display(Name = "Confirmar contraseña")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; }
    }
}
