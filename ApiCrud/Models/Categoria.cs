﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        public bool Status { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        [JsonIgnore]
        public virtual List<Subcategoria> Subcategorias { get; set; }

    }
}
