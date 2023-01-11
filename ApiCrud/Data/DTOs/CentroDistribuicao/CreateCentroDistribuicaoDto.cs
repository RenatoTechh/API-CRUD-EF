using ApiCrud.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiCrud.Data.DTOs
{
    public class CreateCentroDistribuicaoDto
    {
        [Required(ErrorMessage = "Campo nome é obrigatório")]
        [StringLength(128, ErrorMessage = "Máximo de 128 caracteres")]
        [RegularExpression("^[a-zA-Z ]+$")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Campo número é obrigatório")]
        public int Numero { get; set; }
        [Required(ErrorMessage = "Campo cep é obrigatório")]
        public string Cep { get; set; }
    }
}
