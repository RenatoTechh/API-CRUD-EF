using System;
using Xunit;
using UsuariosApi.Services;
using UsuariosApi.Models;
using Microsoft.AspNetCore.Identity;
using UsuariosApi.Data.Requests;
using FluentResults;
using Moq;

namespace LoginTeste
{
    public class LoginTeste
    {
        private SignInManager<CustomIdentityUser> _signInManager;
        private TokenService _tokenService;
            
        Mock<SignInManager<>>

        [Fact]
        public void LoginComSucesso()
        {
            //Arrange
            var request = new LoginRequest();

            request.UserName = "admin";
            request.Password = "Admin123!";

            var loginService = new LoginService(_signInManager, _tokenService);

            //Act
            loginService.LoginUsuario(request);

            //Assert
            Result.Ok();
        }
    }
}
