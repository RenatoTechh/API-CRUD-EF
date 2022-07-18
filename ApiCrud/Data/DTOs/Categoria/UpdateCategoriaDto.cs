using System;
using System.ComponentModel.DataAnnotations;

namespace ApiCrud.Data.DTOs
{
    public class UpdateCategoriaDto
    {
        public string Nome { get; set; }
        public bool Status { get; set; }
        public DateTime DataModificacao { get; set; }
    }
}
