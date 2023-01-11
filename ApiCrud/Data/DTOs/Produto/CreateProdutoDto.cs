using ApiCrud.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ApiCrud.Data.DTOs
{
    public class CreateProdutoDto
    {
        [Required(ErrorMessage = "Campo nome é obrigatório")]
        [StringLength(128, ErrorMessage = "Máximo de 128 caracteres")]
        [RegularExpression("^[a-zA-Z ]+$")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Campo descricao é obrigatório")]
        [StringLength(512, ErrorMessage = "Máximo de 512 caracteres")]
        [RegularExpression("^[a-zA-Z ]+$")]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "Campo peso é obrigatório")]
        public double Peso { get; set; }
        [Required(ErrorMessage = "Campo altura é obrigatório")]
        public double Altura { get; set; }
        [Required(ErrorMessage = "Campo largura é obrigatório")]
        public double Largura { get; set; }
        [Required(ErrorMessage = "Campo comprimento é obrigatório")]
        public double Comprimento { get; set; }
        [Required(ErrorMessage = "Campo valor é obrigatório")]
        public double Valor { get; set; }
        [Required(ErrorMessage = "Campo quantidade em estoque é obrigatório")]
        public int QuantidadeEmEstoque { get; set; }
        [Required(ErrorMessage = "Campo centro de distribuição é obrigatório")]
        public int CentroDistribuicaoId { get; set; }
        public virtual CentroDistribuicao CentroDistribuicao { get; set; }
        [Required]
        public int SubcategoriaId { get; set; }
        public virtual Subcategoria Subcategoria { get; set; }

    }
}
