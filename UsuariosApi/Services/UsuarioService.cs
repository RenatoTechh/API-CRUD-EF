using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using UsuariosApi.Data.Dtos;
using UsuariosApi.Models;
using FluentResults;
using System;
using ApiCrud.Data.DAO;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using UsuariosApi.Data;

namespace UsuariosApi.Services
{
    public class UsuarioService
    {
        private IMapper _mapper;
        private UserManager<CustomIdentityUser> _userManager;
        private EnderecoService _enderecoService;
        private UserDbContext _userDbContext;
        public UsuarioService(IMapper mapper, UserManager<CustomIdentityUser> userManager, EnderecoService enderecoService, UserDbContext userDbContext)
        {
            _mapper = mapper;
            _userManager = userManager;
            _enderecoService = enderecoService;
            _userDbContext = userDbContext;
        }

        public async Task<Result> CadastraUsuarioAsync(CreateUsuarioDto createUsuarioDto)
        {
            var endereco = await _enderecoService.PesquisaEnderecoPorCep(createUsuarioDto.Cep);

            if (endereco.Logradouro == null)
            {
                return Result.Fail("CEP inválido");
            }

            Usuario usuario = _mapper.Map<Usuario>(createUsuarioDto);
            usuario.Status = true;
            usuario.DataCriacao = DateTime.Now;
            usuario.Bairro = endereco.Bairro;
            usuario.Logradouro = endereco.Logradouro;
            usuario.Localidade = endereco.Localidade;
            usuario.Uf = endereco.Uf;

            if (ValidaDataNascimento(createUsuarioDto) == false
               || ValidaCPF(createUsuarioDto.CPF) == false) return Result.Fail("Falha no cadastro do usuário");


            CustomIdentityUser usuarioIdentity = _mapper.Map<CustomIdentityUser>(usuario);

            Task<IdentityResult> resultadoIdentity = _userManager
                .CreateAsync(usuarioIdentity, createUsuarioDto.Password);

            await _userManager.AddToRoleAsync(usuarioIdentity, "regular");

            if (resultadoIdentity.Result.Succeeded)
            {
                return Result.Ok();
            }
            return Result.Fail("Falha ao cadastrar usuário");

        }
        private static bool ValidaDataNascimento(CreateUsuarioDto user)
        {
            if (user.DataNascimento > DateTime.Today)
            {
                return false;
            }
            return true;
        }

        private static bool ValidaCPF(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf;
            string digito;
            int soma;
            int resto;

            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");

            Console.WriteLine("Tamanho: " + cpf.Length);
            if (cpf.Length != 11) { return false; }

            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
            {
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            }

            resto = soma % 11;

            if (resto < 2) { resto = 0; }
            else { resto = 11 - resto; }

            digito = resto.ToString();

            tempCpf += digito;

            soma = 0;

            for (int i = 0; i < 10; i++)
            {
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            }

            resto = soma % 11;

            if (resto < 2) { resto = 0; }
            else { resto = 11 - resto; }

            digito += resto.ToString();

            return cpf.EndsWith(digito);
        }

        public async Task<List<Usuario>> ListarUsuarios(string nome, string cpf, string email, bool? status)
        {
            var usuarios = await _userManager.Users.ToListAsync();

            List<Usuario> listaDeUsuarios = new();

            foreach (var user in usuarios)
            {
                var usuario = _mapper.Map<Usuario>(user);
                listaDeUsuarios.Add(usuario);
            }

            if (nome != null)
            {
                return listaDeUsuarios.Where(u => u.UserName.ToLower().Contains(nome.ToLower())).ToList();
            }
            if (cpf != null)
            {
                return listaDeUsuarios.Where(u => u.CPF == cpf).ToList();
            }
            if (email != null)
            {
                return listaDeUsuarios.Where(u => u.Email == email).ToList();
            }
            if (status != null)
            {
                return listaDeUsuarios.Where(u => u.Status == status).ToList();
            }

            return listaDeUsuarios;
        }
        public async Task<Result> EditarUsuario(int id, UpdateUsuarioDto usuarioDto)
        {
            var usuario = _userManager.Users.FirstOrDefault(c => c.Id == id);

            if (usuario == null)
            {
                return Result.Fail("Usuário não encontrado");
            }


            if (!await ValidaCepAsync(usuarioDto.CEP))
            {
                return Result.Fail("CEP não encontrado");
            }

            var endereco = await _enderecoService.PesquisaEnderecoPorCep(usuarioDto.CEP);

            var usuarioAtualizado = _mapper.Map(usuarioDto, usuario);

            usuarioAtualizado.Cep = endereco.Cep;
            usuarioAtualizado.Bairro = endereco.Bairro;
            usuarioAtualizado.Logradouro = endereco.Logradouro;


            await _userManager.UpdateAsync(usuarioAtualizado);
            await _userDbContext.SaveChangesAsync();

            return Result.Ok();
        }

        public async Task<bool> ValidaCepAsync(string cep)
        {
            var endereco = await _enderecoService.PesquisaEnderecoPorCep(cep);

            if (endereco.Logradouro == null)
            {
                return false;
            }

            return true;
        }

    }
}
