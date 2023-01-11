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
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq;

namespace ApiCrud.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class CentroDistribuicaoController : Controller
    {
        private IMapper _mapper;
        private CentroDistribuicaoDao _dao;
        private ProdutoDao _produtoDao;
        private IConfiguration _configuration;

        public CentroDistribuicaoController(IMapper mapper, CentroDistribuicaoDao dao, ProdutoDao produtoDao, IConfiguration configuration)
        {
            _mapper = mapper;
            _dao = dao;
            _produtoDao = produtoDao;
            _configuration = configuration;
        }
        [HttpPost]
        public async Task<IActionResult> AdicionaCentroDistribuicao([FromBody] CreateCentroDistribuicaoDto CentroDistribuicaoDto)
        {
            var endereco = await _dao.PesquisaEnderecoPorCep(CentroDistribuicaoDto.Cep);

            if (endereco.Logradouro != null)
            {
                var cd = _dao.BuscaCentroDistribuicaoPorNome(CentroDistribuicaoDto.Nome);

                if (cd.Count == 0)
                {
                    _dao.AddCentroDistribuicao(CentroDistribuicaoDto, endereco);

                    // Onde encontrar o objeto que foi criado
                    return CreatedAtAction(nameof(ConsultaCentroDistribuicaoPorNome), new {nome = CentroDistribuicaoDto.Nome }, CentroDistribuicaoDto);
                }
                return BadRequest("O nome do CD já está em uso, digite um nome diferente");
            }

            return BadRequest("CEP não encontrado");
        }
        // GET
        [HttpGet("{nome}")]
        public IActionResult ConsultaCentroDistribuicaoPorNome(string nome, [FromQuery] int skip, [FromQuery] int take)
        {
            var cd = _dao.BuscaCentroDistribuicaoPorNome(nome);

            if (cd.Count == 0)
            {
                return NotFound();
            }

            return Ok(cd.Skip(skip).Take(take));
        }

        [HttpGet("{skip:int}/{take:int}")]
        public async Task<IActionResult> ConsultaCD([FromQuery] string nome, [FromQuery] int? numero, [FromQuery] string cep, [FromQuery] bool? status, [FromQuery] DateTime? dataCriacao, [FromQuery] DateTime? dataModificacao, [FromQuery] string logradouro,
             [FromQuery] string complemento, [FromQuery] string bairro, [FromQuery] string localidade, [FromQuery] string uf, [FromQuery] string order, int skip, int take)
        {
            var data = await _dao.ConsultaCDAsync(nome, numero,cep, status, dataCriacao, dataModificacao, logradouro, complemento, bairro, localidade, uf, order, skip, take);
            if (data.Count == 0)
            {
                return NotFound();
            }

            return Ok(data);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> AtualizaProdutoAsync(int id, [FromBody] UpdateCentroDistribuicaoDto cdDto)
        {
            var cdOrigem = _dao.ConsultaCDPorId(id);

            if (cdOrigem == null)
            {
                return NotFound("CD não encontrado");
            }
            if (String.IsNullOrEmpty(cdDto.Cep))
            {
                cdDto.Cep = cdOrigem.Cep;
            }

            var endereco = await _dao.PesquisaEnderecoPorCep(cdDto.Cep);

            if (endereco.Logradouro == null)
            {
                return BadRequest("CEP não encontrado");
            }
            if (!_dao.ValidaNomeCD(cdDto.Nome))
            {
                return BadRequest("O nome do CD já está em uso, digite um nome diferente");
            }
            if (cdDto.Status == false)
            {
                var verificaProduto = _dao.ConsultaProdutoPorIdCD(cdOrigem.Id);
                var verificaProdutoStatus = verificaProduto.Where(x => x.Status == true).ToList();

                if (verificaProduto.Count == 0 || verificaProdutoStatus.Count == 0)
                {

                    if (cdDto.Cep != cdOrigem.Cep)
                    {
                        _dao.UpdateCentroDistribuicao(cdOrigem, cdDto, endereco);
                        return NoContent();
                    }
                }
                return BadRequest("Não é possível inativar um CD que possui produtos ativos");
            }
            _dao.UpdateCentroDistribuicao(cdOrigem, cdDto, endereco);
            return NoContent();

        }
        [HttpDelete("{Id:int}")]
        public IActionResult ExcluiProduto(int Id)
        {
            var cd = _dao.ConsultaCDPorId(Id);
            if (cd != null)
            {
                _dao.RemoveCD(cd);

                return NoContent();
            }
            return NotFound();
        }
    }
}
