using FluentResults;
using UsuariosApi.Data.Requests;

namespace UsuariosApi.Services
{
    public interface ILogin
    {
        Result LoginUsuario(LoginRequest request);
    }
}
