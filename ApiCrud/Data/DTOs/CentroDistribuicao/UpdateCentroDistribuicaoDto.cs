using System;
using System.ComponentModel.DataAnnotations;

namespace ApiCrud.Data.DTOs
{
    public class UpdateCentroDistribuicaoDto
    {
        public string Nome { get; set; }
        public bool? Status { get; set; }
        public int Numero { get; set; }
        public string Cep { get; set; }

    }
}
