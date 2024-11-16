using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.API.Database.Entities
{
    [Table("movements", Schema = "dbo")]
    public class MovementEntity : BaseEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Range(0.00, float.MaxValue, ErrorMessage = "Ingrese un valor valido...")]
        [Required(ErrorMessage ="Se debe ingresar algo...")]
        [Column("debit")]
        public float Debit {  get; set; }

        [Range(0.00, float.MaxValue, ErrorMessage = "Ingrese un valor valido...")]
        [Required(ErrorMessage = "Se debe ingresar algo...")]
        [Column("credit")]
        public float Credit { get; set; }


        [Column("journalEntry_id")]
        public int JournalId { get; set; }

        [ForeignKey(nameof(JournalId))]
        public virtual JournalEntryEntity JournalEntry { get; set; }


        [Column("account_code")]
        public string AccountCode { get; set; }
        [ForeignKey(nameof(AccountCode))]
        public virtual AccountEntity Account { get; set; }

        public virtual UserEntity CreatedByUser { get; set; }
        public virtual UserEntity UpdatedByUser { get; set; }

    }
}