using ApiCrud.Data;
using ApiCrud.Data.DTOs;
using ApiCrud.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ApiCrud.Data.DAO;

namespace ApiCrud.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class SubcategoriaController : Controller
    {
        private IMapper _mapper;
        private SubcategoriaDao _dao;

        public SubcategoriaController( IMapper mapper, SubcategoriaDao dao)
        {
            _mapper = mapper;
            _dao = dao;
        }
        [HttpPost]
        public IActionResult AdicionaSubcategoria([FromBody] CreateSubcategoriaDto subcategoriaDto)
        {
            Categoria categoria = _dao.BuscaCategoriaIdSubcategoria(subcategoriaDto);

            // Verifica se a consulta da categoria retornouu algum objeto
            if (categoria == null)
            {
                return BadRequest("A categoria não existe");
            }
            // Cadastra a subcategoria somente se a categoria estiver ativa
            if (categoria.Status == true)
            {   
                var subcategoria = _mapper.Map<Subcategoria>(subcategoriaDto);
                subcategoria.DataCriacao = DateTime.Now;
                subcategoria.Status = true;
                _dao.AddSubcategoria(subcategoria);
                // Onde encontrar o objeto que foi criado
                return CreatedAtAction(nameof(ConsultaSubcategoriaPorNome), new { nome = subcategoria.Nome }, subcategoria);
            }

            return BadRequest("A categoria está inativa");
   
        }
        
        [HttpGet("{nome}")]
        public IActionResult ConsultaSubcategoriaPorNome(string nome)
        {
            // Valida o nome da subcategoria
            if (String.IsNullOrEmpty(nome) || nome.Length < 3 || nome.Length > 128)
            {
                return BadRequest("Digite um nome entre 3 e 128 caracteres, somente alfabeto!");
            }

            var subcategoria = _dao.BuscaSubcategoriaPorNome(nome);
            // Verifica se foi encontrada alguma subcaegoria
            if (subcategoria == null)
            {
                return NotFound();
            }
            // 
            var subcategoriaDto = _mapper.Map<List<ReadSubcategoriaDto>>(subcategoria);
            return Ok(subcategoriaDto);
        }
        [HttpGet("{skip:int}/{take:int}")]
        public IActionResult ConsultaSubcategoriaPorStatus([FromQuery] bool? status, [FromQuery] string nome, int skip, int take)
        {

            if (take > 3)
            {
                return BadRequest("Limite atingido");
            }

            var subcategorias = _dao.RetornaTodasSubcategorias();

            if (status == true)
            {
                if (status == true && String.IsNullOrEmpty(nome))
                {
                    return Ok(_dao.RetornaSubcategoriasStatus(skip, take, status));
                }
                var listaSubcategoriasTrue = _dao.RetornaSubcategoriasStatusENome(skip, take, nome, status);

                if (listaSubcategoriasTrue.Count == 0)
                {
                    return NotFound();
                }
                subcategorias = listaSubcategoriasTrue;

                var subcategoriaDto = _mapper.Map<List<ReadSubcategoriaDto>>(subcategorias);
                return Ok(subcategoriaDto);
            }
            
            if (status == false)
            {
                if (String.IsNullOrEmpty(nome))
                {
                    return Ok(_dao.RetornaSubcategoriasStatus(skip, take, status));
                }

                var listaSubcategoriasFalse = _dao.RetornaSubcategoriasStatusENome(skip, take, nome, status);

                if (listaSubcategoriasFalse.Count == 0)
                {
                    return NotFound();
                }

                subcategorias = listaSubcategoriasFalse;

                var subcategoriaDto = _mapper.Map<List<ReadSubcategoriaDto>>(subcategorias);
                return Ok(subcategoriaDto);
            }
            return Ok(subcategorias);
        }
        [HttpPut("{id:int}")]
        public IActionResult AtualizaSubcategoria(int id, [FromBody] UpdateSubcategoriaDto subcategoriaDto)
        {
            var subcategoria = _dao.RetornaSubcategoriaPorId(id);

            if (subcategoria != null)
            {
                _dao.AlteraStatus(subcategoriaDto, subcategoria);

                return NoContent();
            }
            return NotFound();
        }
        [HttpDelete("{Id:int}")]
        public IActionResult ExcluiSubcategoria(int Id)
        {
            var subcategoria = _dao.RetornaSubcategoriaPorId(Id);
            if (subcategoria != null)
            {
                _dao.RemoveSubcategoria(subcategoria);

                return NoContent();
            }
            return NotFound();
        }
    }
}
