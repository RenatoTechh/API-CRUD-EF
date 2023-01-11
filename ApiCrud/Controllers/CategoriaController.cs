using ApiCrud.Data;
using ApiCrud.Data.DAO;
using ApiCrud.Data.DTOs;
using ApiCrud.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiCrud.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class CategoriaController : Controller
    {
        //private CategoriaService _service;
        private IMapper _mapper;
        private CategoriaDao _dao;

        public CategoriaController(/*CategoriaService service,*/ IMapper mapper, CategoriaDao dao)
        {
            //_service = service;
            _mapper = mapper;
            _dao = dao;
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult AdicionaCategoria([FromBody] CreateCategoriaDto categoriaDto)
        {
            var categoria = _dao.AdicionaCategoria(categoriaDto);

            //Onde encontrar o recurso que foi criado
            return CreatedAtAction(nameof(ConsultaCategoriaPorNome), new { nome = categoria.Nome }, categoria);

        }
        [HttpGet]
        [Authorize(Roles = "regular")]
        public IActionResult ConsultaCategorias()
        {
            var categoria = _dao.ListaCategorias();

            if (categoria.Count == 0)
            {
                return NotFound();
            }

            var categoriaDto = _mapper.Map<List<ReadCategoriaDto>>(categoria);
            return Ok(categoriaDto);
        }
        [HttpGet("{nome}")]
        public IActionResult ConsultaCategoriaPorNome(string nome)
        {
            var categoria = _dao.BuscaCategoriaPorNome(nome);
            if (categoria.Count != 0)
            {
                var categoriaDto = _mapper.Map<List<ReadCategoriaDto>>(categoria);
                return Ok(categoriaDto);
            }           
            return NotFound();
        }

        [HttpPut("{CategoriaId:int}")]
        public IActionResult AtualizaCategoria(int CategoriaId, [FromBody] UpdateCategoriaDto categoriaDto)
        {
            var categoria = _dao.BuscaCategoriaPorId(CategoriaId);

            if (categoria != null)
            {
                categoria.DataModificacao = DateTime.Now;

                if (categoriaDto.Status == true)
                {
                    _dao.AtualizaCategoria(categoriaDto, categoria);
                }
                else
                {
                    _dao.AtualizaCategoria(categoriaDto, categoria);
                    // Busca as subcategorias abaixo da categoria
                    var Subcategorias = _dao.BuscaSubcategoriaPorIdCategoria(CategoriaId);

                    foreach (var subcategoria in Subcategorias)
                    {
                        subcategoria.Status = false;
                        subcategoria.DataModificacao = DateTime.Now;
                    }
                }    
                return NoContent();

                //Quando status for false Deve inativar subcategorias
                // Quando categoria não existe Deve retornar 404
                // Quando categoria atualizada Deve retornar 201
                // Quando categoria existe Deve atualiza  dao.Verify(categoria => categoria.AtulizaCategoria), Times.Once();
            }
            return NotFound();
        }
        [HttpDelete("{CategoriaId}")]
        public IActionResult ExcluiCategoria(int categoriaId)
        {
            var categoria = _dao.BuscaCategoriaPorId(categoriaId);
            if (categoria != null)
            {
                _dao.DeletaCategoria(categoria);

                return NoContent();
            }
            return NotFound();
        }
    }
}
