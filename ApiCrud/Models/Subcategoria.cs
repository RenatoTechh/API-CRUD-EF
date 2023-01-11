using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiCrud.Models
{
    public class Subcategoria
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo nome é obrigatório")]
        [StringLength(128, ErrorMessage = "Máximo de 128 caracteres")]
        [RegularExpression("^[a-zA-Z ]+$")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Campo status é obrigatório")]
        public bool? Status { get; set; } = null;
        [Required(ErrorMessage = "Campo DataCriacao é obrigatório")]
        public DateTime DataCriacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        [Required]
        public int CategoriaID { get; set; }
        public virtual Categoria Categoria { get; set; }
        [JsonIgnore]
        public virtual List<Produto> Produtos { get; set; }
    }
}
