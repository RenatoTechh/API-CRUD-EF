using ApiCrud.Data.DTOs;
using ApiCrud.Models;
using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCrud.Data.DAO
{
    public class ProdutoDao
    {
        AppDbContext _context;
        IMapper _mapper;

        private IConfiguration _configuration; 
        public ProdutoDao(AppDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }
        //  GET
        public Subcategoria BuscaSubcategoriaPorIdProduto([FromBody] CreateProdutoDto produtoDto)
        {
            // Verifica o Id da Categoria com a chave estrangeira da subcategoria
            return _context.Subcategorias.FirstOrDefault(sub => sub.Id == produtoDto.SubcategoriaId);
        }
        // POST
        public void AddProduto(CreateProdutoDto produtoDto)
        {
            var produto = _mapper.Map<Produto>(produtoDto);
            produto.Status = true;
            produto.DataCriacao = DateTime.Now;
            _context.Produtos.Add(produto);
            _context.SaveChanges();
        }
        //  GET
        public List<Produto> BuscaProdutoPorNome(string nome)
        {
            // Realiza a busca no banco através do nome
            return _context.Produtos.Where(produto => produto.Nome.ToLower().Equals(nome.ToLower())).ToList();
        }
        //  GET
        public Produto RetornaProdutoPorId(int id)
        {
            //Produto produto;
            //using (var connection = new MySqlConnection(_configuration.GetConnectionString("AppConnection")))
            //{
            //    produto = connection.QueryFirstOrDefault<Produto>(@"SELECT * FROM produtos WHERE Id =" + id);
            //}

            var produto = _context.Produtos.FirstOrDefault(_produto => _produto.Id == id);

            return produto;
        }
        // PUT
        public void AlteraStatusProduto(UpdateProdutoDto produtoDto, Produto produto)
        {
            produto.DataModificacao = DateTime.Now;
            _mapper.Map(produtoDto, produto);
            _context.SaveChanges();
        }
        // DELETE
        public void RemoveProduto(Produto produto)
        {
            _context.Produtos.Remove(produto);
            _context.SaveChanges();
        }
        public async Task<List<Produto>> ConsultaProdutoAsync(string nome, bool? status, double? peso, double? altura, double? largura, double? comprimento,
             double? valor, int? quantidadeEmEstoque, string centroDeDistribuicao, string order, int skip, int take)
        {
            var sql = "SELECT * FROM Produtos WHERE " +
                "[produtosId] Id";

            var parametros = new DynamicParameters();

            if (peso != null)
            {
                sql += "Peso = @peso and ";
            }

            if (altura != null)
            {
                sql += "Altura = @altura and ";
            }
            if (nome != null)
            {
                sql += "Nome LIKE \"%"+nome+"%\" and ";
            }
            if (status != null)
            {
                sql += "Status = @status and ";
            }
            if (largura != null)
            {
                sql += "Largura = @largura and ";
            }
            if (comprimento != null)
            {
                sql += "Comprimento = @comprimento and ";
            }
            if (valor != null)
            {
                sql += "Valor = @valor and ";
            }
            if (quantidadeEmEstoque != null)
            {
                sql += "QuantidadeEmEstoque = @quantidadeEmEstoque and ";
            }
            if (centroDeDistribuicao != null)
            {
                sql += "CentroDeDistribuicao = @centroDeDistribuicao and";
            }

            if (peso == null && nome == null && altura == null && status == null
                 && largura == null && comprimento == null && valor == null
                 && quantidadeEmEstoque == null && centroDeDistribuicao == null)
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
                sql += "order by produtos.nome desc";
            }

            if (order != "desc")
            {
                sql += "order by produtos.nome";
            }

            Console.WriteLine(sql);
            IEnumerable<Produto> produto;
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("AppConnection")))
            {
                connection.Open();

                produto = await connection.QueryAsync<Produto>(sql, new 
                {
                    Nome = nome,
                    Status = status,
                    Peso = peso,
                    Altura = altura,
                    Largura = largura,
                    Comprimento = comprimento,
                    Valor = valor,
                    QuantidadeEmEstoque = quantidadeEmEstoque,
                    CentroDeDistribuicao = centroDeDistribuicao
                });
            }

            return produto.Skip(skip).Take(take).ToList();
        }
    }
}
