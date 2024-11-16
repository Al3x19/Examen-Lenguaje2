using SistemaContable.API.Dtos.Movements;
using System.ComponentModel.DataAnnotations;

namespace SistemaContable.API.Dtos.JournalEntries
{
    public class JournalEntryUpdateDto 
    {

        [Display(Name = "description")]
        [StringLength(500, ErrorMessage = "El {0} no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        public string Description { get; set; }

    }
}
