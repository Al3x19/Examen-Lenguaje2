using SistemaContable.API.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaContable.API.Dtos.Movements
{
    public class MovementCreateDto
    {
        [Display(Name = "debit")]
        [Range(0.00, float.MaxValue, ErrorMessage = "Ingrese un valor valido...")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        public float Debit { get; set; }

        [Display(Name = "credit")]
        [Range(0.00, float.MaxValue, ErrorMessage = "Ingrese un valor valido...")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        public float Credit { get; set; }

        [Display(Name = "journalEntry_id")]
        public int JournalId { get; set; }

        [Display(Name = "account_code")]
        public string AccountCode { get; set; }

    }
}
