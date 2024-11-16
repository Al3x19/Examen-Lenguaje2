using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContable.API.Database.Entities
{
    [Table("Logs", Schema = "dbo")]
    public class LogsEntity : BaseEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("action")]
        public string action { get; set; }

        [Column("old_value")]
        public string oldValue { get; set; }

        [Column("new_value")]
        public string newValue { get; set; }

        [Column("table_affected")]
        public string table {  get; set; }

        //public virtual UserEntity CreatedByUser { get; set; }
        //public virtual UserEntity UpdatedByUser { get; set; }

    }
}
