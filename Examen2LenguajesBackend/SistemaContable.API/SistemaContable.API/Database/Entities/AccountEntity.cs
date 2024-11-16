using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.API.Database.Entities
{
    [Table("accounts", Schema = "dbo")]
    public class AccountEntity : BaseEntity
    {
        [Key]
        [Column("code")]
        public string Code { get; set; }

        [StringLength(50)]
        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [Column("allow_movement")]
        public bool AllowMovement { get; set; }

        [Required]
        [Column("is_blocked")]
        public bool IsBlocked { get; set; }

        [Column("parent_account")]
        public string ParentAccountCode { get; set; }
        [ForeignKey(nameof(ParentAccountCode))]
        public virtual AccountEntity ParentAccount { get; set; }
        public virtual UserEntity CreatedByUser { get; set; }
        public virtual UserEntity UpdatedByUser { get; set; }

    }
}