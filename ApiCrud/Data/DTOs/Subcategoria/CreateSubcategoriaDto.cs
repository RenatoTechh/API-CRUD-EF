using ApiCrud.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiCrud.Data.DTOs
{
    public class CreateSubcategoriaDto
    {
        [Required(ErrorMessage = "Campo nome é obrigatório")]
        [StringLength(128, ErrorMessage = "Máximo de 128 caracteres")]
        public string Nome { get; set; }
        [Required]
        public int CategoriaID { get; set; }

    }
}
