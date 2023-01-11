using ApiCrud.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiCrud.Data.DTOs
{
    public class CreateCategoriaDto
    {
        [Required(ErrorMessage = "Campo nome é obrigatório")]
        [StringLength(128, ErrorMessage = "Máximo de 128 caracteres")]
        public string Nome { get; set; }
    }
}
