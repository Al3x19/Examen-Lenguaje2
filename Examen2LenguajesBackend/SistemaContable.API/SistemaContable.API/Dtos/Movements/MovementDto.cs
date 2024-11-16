using Microsoft.AspNetCore.Identity;
using SistemaContable.API.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SistemaContable.API.Dtos.Accounts;

namespace SistemaContable.API.Dtos.Movements
{
    public class MovementDto
    {


        public float Debit { get; set; }


        public float Credit { get; set; }


        public AccountDto Account { get; set; }

    }
}
