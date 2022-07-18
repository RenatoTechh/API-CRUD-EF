using ApiCrud.Data;
using ApiCrud.Data.DTOs;
using ApiCrud.Models;
using AutoMapper;
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
        private AppDbContext _context;
        private IMapper _mapper;

        public CategoriaController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpPost]
        public IActionResult AdicionaCategoria([FromBody] CreateCategoriaDto categoriaDto)
        {
            var categoria = _mapper.Map<Categoria>(categoriaDto);

            categoria.Status = true;
            categoria.DataCriacao = DateTime.Now;
            
            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            //Onde encontrar o recurso que foi criado
            return CreatedAtAction(nameof(ConsultaCategoriaPorNome), new { nome = categoria.Nome }, categoria);
        }
        [HttpGet]
        public IActionResult ConsultaCategorias()
        {
            var categoria = _context.Categorias.OrderBy(categoria => categoria.Nome);
            var categoriaDto = _mapper.Map<List<ReadCategoriaDto>>(categoria);

            return Ok(categoriaDto);
        }
        [HttpGet("{nome}/{skip:int}/{take:int}")]
        public IActionResult ConsultaCategoriaPorNome(string nome, int skip, int take)
        {
            var categoria = _context.Categorias.Skip(skip).Take(take)
                .Where(categoria => categoria.Nome.ToLower() == nome.ToLower()).ToList();
            if (categoria != null)
            {
                var categoriaDto = _mapper.Map<List<ReadCategoriaDto>>(categoria);
                return Ok(categoriaDto);
            }           
            return NotFound();
        }

        [HttpPut("{CategoriaId:int}")]
        public IActionResult AtualizaCategoria(int CategoriaId, [FromBody] UpdateCategoriaDto categoriaDto)
        {
            var categoria = _context.Categorias.FirstOrDefault(categoria => categoria.CategoriaId == CategoriaId);

            if (categoria != null)
            {
                if (categoriaDto.Status == true)
                {
                    categoriaDto.DataModificacao = DateTime.Now;
                    _mapper.Map(categoriaDto, categoria);
                }
                else
                {
                    categoriaDto.DataModificacao = DateTime.Now;
                    _mapper.Map(categoriaDto, categoria);

                    var Subcategorias = _context.Subcategorias.Where(subcategoria => subcategoria.CategoriaID == CategoriaId).ToList();

                    foreach (var subcategoria in Subcategorias)
                    {
                        subcategoria.Status = false;
                        subcategoria.DataModificacao = DateTime.Now;
                    }
                    _context.SaveChanges();
                }
                return NoContent();
            }
            return NotFound();
        }
        [HttpDelete("{CategoriaId}")]
        public IActionResult ExcluiCategoria(int categoriaId)
        {
            var categoria = _context.Categorias.FirstOrDefault(categoria => categoria.CategoriaId == categoriaId);
            if (categoria != null)
            {
                _context.Categorias.Remove(categoria);
                _context.SaveChanges();

                return NoContent();
            }
            return NotFound();
        }
    }
}
