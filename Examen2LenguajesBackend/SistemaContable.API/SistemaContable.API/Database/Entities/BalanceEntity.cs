using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SistemaContable.API.Database.Entities
{
    [Table("balances", Schema = "dbo")]
    public class BalanceEntity : BaseEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }


        [Column("month")]
        [Required(ErrorMessage ="Se requiere el mes...")]
        [Range(1, 12, ErrorMessage = "ingrese un mes valido.")]
        public int Month { get; set; }

        [Column("year")]
        [Required(ErrorMessage = "Se requiere el año...")]
        public int Year { get; set; }

        [Range(0.00, float.MaxValue, ErrorMessage = "Valor invalido...")]
        [Required(ErrorMessage = "Se debe ingresar algo...")]
        [Column("amount")]
        public float Amount { get; set; }

        [Column("account_code")]
        public string AccountCode { get; set; }
        [ForeignKey(nameof(AccountCode))]
        public virtual AccountEntity Account { get; set; }

        public virtual UserEntity CreatedByUser { get; set; }
        public virtual UserEntity UpdatedByUser { get; set; }
    }
}
