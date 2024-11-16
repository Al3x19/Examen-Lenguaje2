using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SistemaContable.API.Database.Entities
{
    [Table("journal_entries", Schema = "dbo")]
    public class JournalEntryEntity : BaseEntity
    {

        [Key]
        [Column("number")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Number{ get; set; }


        [StringLength(500)]
        [Required]
        [Column("description")]
        public string Description{ get; set; }


        [Required]
        [Column("is_hidden")]
        public bool IsHidden { get; set; }

        public virtual IEnumerable<MovementEntity> Movements { get; set; }
        public virtual UserEntity CreatedByUser { get; set; }
        public virtual UserEntity UpdatedByUser { get; set; }
    }
}
