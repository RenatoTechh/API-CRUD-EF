using ApiCrud.Data.DAO;
using ApiCrud.Data.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ApiCrud.Models;
using Dapper;
using System.Threading.Tasks;

namespace ApiCrud.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ProdutoController : Controller
    {
        private IMapper _mapper;
        private ProdutoDao _dao;
        private IConfiguration _configuration;

        public ProdutoController(IMapper mapper, ProdutoDao dao, IConfiguration configuration)
        {
            _mapper = mapper;
            _dao = dao;
            _configuration = configuration;
        }
        [HttpPost]
        public IActionResult AdicionaProduto([FromBody] CreateProdutoDto produtoDto)
        {
            var subcategoria = _dao.BuscaSubcategoriaPorIdProduto(produtoDto);

            if (subcategoria.Status == true)
            {
                var produto = _dao.BuscaProdutoPorNome(produtoDto.Nome);
                if (produto.Count == 0)
                {
                    _dao.AddProduto(produtoDto);

                    // Onde encontrar o objeto que foi criado
                    return CreatedAtAction(nameof(ConsultaProdutoPorNome), new { nome = produtoDto.Nome }, produtoDto);
                }

                return BadRequest("O nome do produto já está em uso");
            }
            // Verifica se a consulta da subcategoria retornou algum objeto
            if (subcategoria == null)
            {
                return NotFound("A subcategoria não existe");
            }

            return BadRequest("A subcategoria está inativa");
        }
        
        [HttpGet("{nome}")]
        public IActionResult ConsultaProdutoPorNome(string nome)
        {
            var produto = _dao.BuscaProdutoPorNome(nome);

            if (produto == null)
            {
                return NotFound("Produto não encontrado");
            }

            var produtoDto = _mapper.Map<List<ReadProdutoDto>>(produto);
            return Ok(produtoDto);
        }
        [HttpGet("{skip:int}/{take:int}")]
        public async Task<IActionResult> ConsultaProduto([FromQuery] string nome, [FromQuery] bool? status, [FromQuery] double? peso, [FromQuery] double? altura, [FromQuery] double? largura, [FromQuery] double? comprimento,
             [FromQuery] double? valor, [FromQuery] int? quantidade, [FromQuery] string centro,[FromQuery] string order, int skip, int take)
        {
            var data = await _dao.ConsultaProdutoAsync(nome, status, peso, altura, largura, comprimento, valor, quantidade, centro, order, skip, take);
            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }
        [HttpPut("{id:int}")]
        public IActionResult AtualizaProduto(int id, [FromBody] UpdateProdutoDto produtoDto)
        {
            var produto = _dao.RetornaProdutoPorId(id);

            if (produto != null)
            {
                _dao.AlteraStatusProduto(produtoDto, produto);

                return NoContent();
            }
            return NotFound();
        }
        [HttpDelete("{Id:int}")]
        public IActionResult ExcluiProduto(int Id)
        {
            var subcategoria = _dao.RetornaProdutoPorId(Id);
            if (subcategoria != null)
            {
                _dao.RemoveProduto(subcategoria);

                return NoContent();
            }
            return NotFound();
        }
    }
}
