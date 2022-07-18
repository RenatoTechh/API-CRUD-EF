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
        [Required]
        public bool Status { get; set; }
        [Required]
        public DateTime DataCriacao { get; set; }
        public DateTime DataModificacao { get; set; }
        public virtual List<Subcategoria> Subcategorias { get; set; }
    }
}
