using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UsuariosApi.Data.Dtos;
using UsuariosApi.Services;

namespace UsuariosApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost]
        public async Task<IActionResult> CadastraUsuarioAsync(CreateUsuarioDto createDto)
        {
            var resultado = await _usuarioService.CadastraUsuarioAsync(createDto);
            if (resultado.IsFailed)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ListarUsuarios([FromQuery] string nome,
            [FromQuery] string cpf, [FromQuery] string email, [FromQuery] bool? status)
        {
            var resultado = await _usuarioService.ListarUsuarios(nome, cpf, email, status);
            return Ok(resultado);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "admin, regular")]
        public async Task<IActionResult> EditarUsuario(int id, [FromBody] UpdateUsuarioDto userDto)
        {
            var retornoRole = await _usuarioService.EditarUsuario(id, userDto);
            if (!retornoRole.IsSuccess) return BadRequest();
            return Ok("Usuário alterado");
        }
    }
}

