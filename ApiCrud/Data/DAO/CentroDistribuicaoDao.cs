using ApiCrud.Data.DTOs;
using ApiCrud.Models;
using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiCrud.Data.DAO
{
    public class CentroDistribuicaoDao
    {
        AppDbContext _context;
        IMapper _mapper;
        private IConfiguration _configuration;
        public CentroDistribuicaoDao(AppDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }
        
        // POST
        public void AddCentroDistribuicao(CreateCentroDistribuicaoDto centroDistribuicaoDto, CentroDistribuicao endereco)
        {
            var cd = _mapper.Map<CentroDistribuicao>(centroDistribuicaoDto);
            cd.Status = true;
            cd.DataCriacao = DateTime.Now;
            cd.Logradouro = endereco.Logradouro;
            cd.Bairro = endereco.Bairro;
            cd.Localidade = endereco.Localidade;
            cd.Uf = endereco.Uf;
            _context.CentrosDistribuicao.Add(cd);
            _context.SaveChanges();
        }
        // PUT
        public void UpdateCentroDistribuicao(CentroDistribuicao cdOrigem, UpdateCentroDistribuicaoDto centroDistribuicaoDto, CentroDistribuicao endereco)
        {
            if (centroDistribuicaoDto.Nome == null)
            {
                centroDistribuicaoDto.Nome = cdOrigem.Nome;
            }
            if (centroDistribuicaoDto.Numero == 0)
            {
                centroDistribuicaoDto.Numero = cdOrigem.Numero;
            }
            if (centroDistribuicaoDto.Status == null)
            {
                centroDistribuicaoDto.Status = cdOrigem.Status;
            }

            var cd = _mapper.Map(centroDistribuicaoDto, cdOrigem);

            cd.DataModificacao = DateTime.Now;
            cd.Logradouro = endereco.Logradouro;
            cd.Bairro = endereco.Bairro;
            cd.Localidade = endereco.Localidade;
            cd.Uf = endereco.Uf;

            _context.SaveChanges();
        }
        public bool ValidaNomeCD(string nome)
        {
            if (String.IsNullOrEmpty(nome))
            {
                return true;
            }

            var cd = BuscaCentroDistribuicaoPorNome(nome);
            if (cd.Count == 0)
            {
                return true;
            }

            return false;
        }
        // GET
        public List<Produto> ConsultaProdutoPorIdCD(int id)
        {
            var produto = _context.Produtos.Where(p => p.CentroDistribuicaoId == id);

            return produto.ToList();
        }
        //  GET
        public List<ReadCentroDistribuicaoDto> BuscaCentroDistribuicaoPorNome( string nome)
        {
            var cd = _context.CentrosDistribuicao.Where(cd => cd.Nome.ToLower().Equals(nome.ToLower())).ToList();

            var cdDto = _mapper.Map<List<ReadCentroDistribuicaoDto>>(cd);

            return cdDto;
        }
        //  GET
        public async Task<CentroDistribuicao> PesquisaEnderecoPorCep(string cep)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync($"https://viacep.com.br/ws/{cep}/json/");
            var content = await response.Content.ReadAsStringAsync();

            var endereco = JsonConvert.DeserializeObject<CentroDistribuicao>(content);

            return endereco;
        }

        // DELETE
        public void RemoveCD(CentroDistribuicao cd)
        {
            _context.CentrosDistribuicao.Remove(cd);
            _context.SaveChanges();
        }
        public CentroDistribuicao ConsultaCDPorId(int id)
        {
            var centroDistribuicao = _context.CentrosDistribuicao.FirstOrDefault(cd => cd.Id == id);

            return centroDistribuicao;
        }

        // GET
        public async Task<List<CentroDistribuicao>> ConsultaCDAsync(string nome, int? numero, string? cep, bool? status, DateTime? dataCriacao, DateTime? dataModificacao,
             string logradouro, string complemento, string bairro, string localidade, string? uf, string order, int skip, int take)
        {
            var sql = "SELECT * FROM CentrosDistribuicao WHERE ";

            if (nome != null)
            {
                sql += "Nome LIKE \"%"+nome+"%\" and ";
            }

            if (numero != null)
            {
                sql += "Numero = @numero and ";
            }
            if (cep != null)
            {
                sql += "Cep = @cep and ";
            }
            if (status != null)
            {
                sql += "Status = @status and ";
            }
            if (dataCriacao != null)
            {
                sql += "DATE_FORMAT(DataCriacao ,'%d/%m/%Y') = @dataCriacao and ";
            }
            if (dataModificacao != null)
            {
                sql += "DATE_FORMAT(DataModificacao ,'%d/%m/%Y') = @dataModificacao and ";
            }
            if (logradouro != null)
            {
                sql += "Logradouro = @logradouro and ";
            }
            if (complemento != null)
            {
                sql += "Complemento = @complemento and ";
            }
            if (bairro != null)
            {
                sql += "Bairro = @bairro and";
            }
            if (uf != null)
            {
                sql += "Uf = @uf and";
            }

            if (nome == null && numero == null && cep == null && status == null
                 && dataCriacao == null && dataModificacao == null && logradouro == null
                 && complemento == null && bairro == null && uf == null)
            {
                var posicaoWhere = sql.LastIndexOf("WHERE");
                sql = sql.Remove(posicaoWhere);

            }
            else
            {
                var posicaoAnd = sql.LastIndexOf("and");
                sql = sql.Remove(posicaoAnd);
            }

            if (order == "desc")
            {
                sql += "order by centrosDistribuicao.nome desc";
            }

            if (order != "desc")
            {
                sql += "order by centrosDistribuicao.nome";
            }

            IEnumerable<CentroDistribuicao> cd;
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("AppConnection")))
            {
                cd = await connection.QueryAsync<CentroDistribuicao>(sql, new
                {
                    Nome = nome,
                    Numero = status,
                    Cep = cep,
                    DataCriacao = dataCriacao,
                    DataModificacao = dataModificacao,
                    Logradouro = logradouro,
                    Complemento = complemento,
                    Bairro = bairro,
                    Uf = uf
                });
            }

            return cd.Skip(skip).Take(take).ToList();
        }
    }
}
