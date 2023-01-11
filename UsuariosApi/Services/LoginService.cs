using FluentResults;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using UsuariosApi.Data.Requests;
using UsuariosApi.Models;

namespace UsuariosApi.Services
{
    public class LoginService
    {
        private SignInManager<CustomIdentityUser> _signInManager;
        private TokenService _tokenService;

        public LoginService(SignInManager<CustomIdentityUser> signInManager, TokenService tokenService)
        {
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public Result LoginUsuario(LoginRequest request)
        {
            var resultadoIdentity = _signInManager.PasswordSignInAsync(request.UserName, request.Password, false, false);
            if (resultadoIdentity.Result.Succeeded)
            {
                var identityUser = _signInManager
                    .UserManager
                    .Users
                    .FirstOrDefault(user => user.NormalizedUserName == request.UserName.ToUpper());

                Token token = _tokenService.CreateToken(identityUser, _signInManager
                    .UserManager.GetRolesAsync(identityUser).Result.FirstOrDefault());

                return Result.Ok().WithSuccess(token.Value);
            }

            return Result.Fail("Login falhou");
        }
    }
}
