using ApiCrud.Data;
using ApiCrud.Models;
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
        private CategoriaContext _context;

        public CategoriaController(CategoriaContext context)
        {
            _context = context;
        }
        [HttpPost]
        public IActionResult AdicionaCategoria([FromBody] Categoria categoria)
        {
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
 
            return Ok(_context.Categorias.OrderBy(categoria => categoria.Nome));
        }
        [HttpGet("{nome}")]
        public IActionResult ConsultaCategoriaPorNome(string nome)
        {
            var categoria = _context.Categorias.FirstOrDefault(categoria => categoria.Nome.ToLower() == nome.ToLower());
            if (categoria != null)
            {
                return Ok(categoria);
            }           
            return NotFound();
        }
        [HttpPut("{nome}")]
        public IActionResult AtualizaCategoria(string nome, [FromBody] Categoria novaCategoria)
        {
            var categoria = _context.Categorias.FirstOrDefault(categoria => categoria.Nome.ToLower() == nome.ToLower());
            if (categoria != null)
            {
                categoria.Nome = novaCategoria.Nome;
                categoria.DataModificacao = novaCategoria.DataModificacao;
                _context.SaveChanges();

                return NoContent();
            }
            return NotFound();
        }
    }
}
