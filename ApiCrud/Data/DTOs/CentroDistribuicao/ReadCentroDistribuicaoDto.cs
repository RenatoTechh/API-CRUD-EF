using ApiCrud.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiCrud.Data.DTOs
{
    public class ReadCentroDistribuicaoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Numero { get; set; }
        public string Cep { get; set; }
        public bool? Status { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Localidade { get; set; }
        public string Uf { get; set; }
        public virtual List<ReadProdutoDto> Produtos { get; set; }

    }
}
