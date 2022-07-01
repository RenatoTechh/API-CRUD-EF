using System;
using System.ComponentModel.DataAnnotations;

namespace ApiCrud.Models
{
    public class Categoria
    {
        [Key]
        [Required]
        public int CategoriaId { get; set; }
        [Required(ErrorMessage = "Campo nome é obrigatório")]
        [StringLength(128,ErrorMessage = "Máximo de 128 caracteres")]
        public string Nome { get; set; }
        [Required]
        public bool Status { get; set; }
        [Required]
        public DateTime DataCriacao { get; set; }
        public DateTime DataModificacao { get; set; }
    }
}
