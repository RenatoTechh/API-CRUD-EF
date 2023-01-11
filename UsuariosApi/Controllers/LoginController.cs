using Microsoft.AspNetCore.Mvc;
using System.Linq;
using UsuariosApi.Data.Requests;
using UsuariosApi.Services;

namespace UsuariosApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private LoginService _loginService;

        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public IActionResult LogaUsuario(LoginRequest request)
        {
            var result = _loginService.LoginUsuario(request);
            if (result.IsFailed)
            {
                return Unauthorized("Usuário ou senha incorreta");
            }

            return Ok(result.Successes.FirstOrDefault());

        }
    }
}
