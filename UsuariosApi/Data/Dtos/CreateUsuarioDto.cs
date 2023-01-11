using System;
using System.ComponentModel.DataAnnotations;

namespace UsuariosApi.Data.Dtos
{
    public class CreateUsuarioDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Senhas divergentes")]
        public string RePassword { get; set; }
        [Required]
        public string CPF { get; set; }
        [Required]
        public DateTime DataNascimento { get; set; }
        [Required]
        public int Numero { get; set; }
        [Required]
        public string Cep { get; set; }
        public string Complemento { get; set; }

    }
}
