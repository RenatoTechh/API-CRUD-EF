using ApiCrud.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiCrud.Data.DTOs
{
    public class ReadProdutoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public double Peso { get; set; }
        public double Altura { get; set; }
        public double Largura { get; set; }
        public double Comprimento { get; set; }
        public double Valor { get; set; }
        public bool Status { get; set; }
        public int QuantidadeEmEstoque { get; set; }
        [JsonIgnore]
        public CentroDistribuicao CentroDistribuicao { get; set; }
        public Subcategoria Subcategoria { get; set; }

    }
}
