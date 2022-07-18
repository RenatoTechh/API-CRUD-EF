using ApiCrud.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiCrud.Data.DTOs
{
    public class ReadCategoriaDto
    {
        public int CategoriaId { get; set; }
        public string Nome { get; set; }
        public bool Status { get; set; }
        public string DataCriacao { get; set; }
        public DateTime DataModificacao { get; set; }
        public Object Subcategorias { get; set; }
    }
}
