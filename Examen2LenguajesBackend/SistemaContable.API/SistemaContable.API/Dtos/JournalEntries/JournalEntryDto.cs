using SistemaContable.API.Database.Entities;
using SistemaContable.API.Dtos.Movements;
using System.ComponentModel.DataAnnotations;

namespace SistemaContable.API.Dtos.JournalEntries
{
    public class JournalEntryDto
    {

        public int Number { get; set; }

        public string Description { get; set; }

        public virtual List<MovementDto> Movements { get; set; } = new List<MovementDto>();

        public DateTime CreatedDate { get; set; }

        public string User { get; set; }
    }
}
