using SistemaContable.API.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SistemaContable.API.Dtos.Movements;

namespace SistemaContable.API.Dtos.JournalEntries
{
    public class JournalEntryCreateDto
    {

        [Display(Name = "description")]
        [StringLength(500, ErrorMessage = "El {0} no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        public string Description { get; set; }

        [Display(Name = "movimientos")]
        [Required(ErrorMessage = "los {0} son requeridos.")]
        [MinLength(2, ErrorMessage = "Debe haber al menos dos movimientos.")]
        public virtual List<MovementCreateDto> Movements { get; set; }
    }
}
