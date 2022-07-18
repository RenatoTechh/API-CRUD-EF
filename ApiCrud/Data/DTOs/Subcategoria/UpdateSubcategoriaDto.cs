using System;
using System.ComponentModel.DataAnnotations;

namespace ApiCrud.Data.DTOs
{
    public class UpdateSubcategoriaDto
    {
        
        [Required(ErrorMessage = "Campo status é obrigatório")]
        public bool Status { get; set; }
        [Required(ErrorMessage = "Campo DataModificacao é obrigatório")]
        public DateTime DataModificacao { get; set; }
    }
}
