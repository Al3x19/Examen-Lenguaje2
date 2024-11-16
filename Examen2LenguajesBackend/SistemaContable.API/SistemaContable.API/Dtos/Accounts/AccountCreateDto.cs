using Microsoft.AspNetCore.Identity;
using SistemaContable.API.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaContable.API.Dtos.Accounts
{
    public class AccountCreateDto
    {
       
        [Display(Name = "name")]
        [StringLength(50, ErrorMessage = "El {0} no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        public string Name { get; set; }
        

       
        //[Display(Name = "allow_movement")]
        //[Required(ErrorMessage = " {0} es requerido.")]
        //public bool AllowMovement { get; set; }

        [Display(Name = "parent_account")]
        public string ParentAccountCode { get; set; }
 

    }
}
