using ApiCrud.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiCrud.Data.DTOs
{
    public class ReadSubcategoriaDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool Status { get; set; }
        public string DataCriacao { get; set; }
        public DateTime DataModificacao { get; set; }
        public Categoria Categoria { get; set; }
        public object Produtos { get; set; }

    }
}
