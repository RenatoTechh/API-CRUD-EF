using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiCrud.Models
{
    public class CentroDistribuicao
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo nome é obrigatório")]
        [StringLength(128, ErrorMessage = "Máximo de 128 caracteres")]
        [RegularExpression("^[a-zA-Z ]+$")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Campo número é obrigatório")]
        public int Numero { get; set; }
        public string Cep { get; set; }
        public bool Status { get; set; }
        [Required(ErrorMessage = "Campo DataCriacao é obrigatório")]
        public DateTime DataCriacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        [JsonIgnore]
        public virtual List<Produto> Produtos { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Localidade { get; set; }
        public string Uf { get; set; }
    }
}
