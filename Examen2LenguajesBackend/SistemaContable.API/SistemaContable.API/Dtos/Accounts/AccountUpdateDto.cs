using System.ComponentModel.DataAnnotations;

namespace SistemaContable.API.Dtos.Accounts
{
    public class AccountUpdateDto 
    {
        [Display(Name = "name")]
        [StringLength(50, ErrorMessage = "El {0} no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        public string Name { get; set; }


    }
}
