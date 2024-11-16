using SistemaContable.API.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaContable.API.Dtos.Accounts
{
    public class AccountDto
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public bool AllowMovement { get; set; }

        public virtual AccountEntity ParentAccount { get; set; }
    }
}
