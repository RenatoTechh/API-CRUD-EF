using ApiCrud.Data;
using ApiCrud.Data.DTOs;
using ApiCrud.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ApiCrud.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class SubcategoriaController : Controller
    {
        private AppDbContext _context;
        private IMapper _mapper;

        public SubcategoriaController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpPost]
        public IActionResult AdicionaSubcategoria([FromBody] CreateSubcategoriaDto subcategoriaDto)
        {
            if (true)
            {

            }   
            // Verifica o Id da Categoria com a chave estrangeira da subcategoria
            var categoria = _context.Categorias.FirstOrDefault(categoria => categoria.CategoriaId == subcategoriaDto.CategoriaID);
            // Verifica se a consulta da categoria retornouu algum objeto
            if (categoria == null)
            {
                return BadRequest("A categoria não existe");
            }
            // Cadastra a subcategoria somente se a categoria estiver ativa
            if (categoria.Status == true)
            {
                subcategoriaDto.DataCriacao = DateTime.Now;
                Subcategoria subcategoria = _mapper.Map<Subcategoria>(subcategoriaDto);
                _context.Subcategorias.Add(subcategoria);
                _context.SaveChanges();
                // Onde encontrar o recurso que foi criado
                return CreatedAtAction(nameof(ConsultaSubcategoriaPorNome), new { nome = subcategoria.Nome }, subcategoria);
            }

            return BadRequest("A categoria está inativa");

            
        }
        [HttpGet("{skip:int}/{take:int}")]
        public IActionResult ConsultaSubcategorias(int skip, int take)
        {
            if (take > 3)
            {
                return BadRequest("Limite atingido");
            }
            var subcategorias = _context.Subcategorias.Skip(skip).Take(take).ToList();

            return Ok(subcategorias.OrderBy(subcategoria => subcategoria.Nome));

        }
        [HttpGet("{nome}")]
        public IActionResult ConsultaSubcategoriaPorNome(string nome)
        {
            // Valida o nome da subcategoria
            if (String.IsNullOrEmpty(nome) || nome.Length < 3 || nome.Length > 128)
            {
                return BadRequest("Digite um nome entre 3 e 128 caracteres, somente alfabeto!");
            }

            // Realiza a busca no banco através do nome
            var subcategoria = _context.Subcategorias
                .Where(subcategoria => subcategoria.Nome.ToLower() == nome.ToLower()).ToList();
            // Verifica se foi encontrada alguma subcaegoria
            if (subcategoria == null)
            {
                return NotFound();
            }
            // 
            var subcategoriaDto = _mapper.Map<List<ReadSubcategoriaDto>>(subcategoria);
            return Ok(subcategoriaDto);
        }
        [HttpGet("{status:bool}/{skip:int}/{take:int}")]
        public IActionResult ConsultaSubcategoriaPorStatus(bool status, int skip, int take)
        {
            if (take > 3)
            {
                return BadRequest("Limite atingido");
            }
            // Realiza a busca no banco através do status
            var subcategoria = _context.Subcategorias.Skip(skip).Take(take)
                .Where(subcategoria => subcategoria.Status == status).ToList();
            // Verifica se foi encontrada alguma subcaegoria
            if (subcategoria == null)
            {
                return NotFound();
            }
            // 
            var subcategoriaDto = _mapper.Map<List<ReadSubcategoriaDto>>(subcategoria);
            return Ok(subcategoriaDto);
        }
        [HttpPut("{id:int}")]
        public IActionResult AtualizaSubcategoria(int id, [FromBody] UpdateSubcategoriaDto subcategoriaDto)
        {
            var subcategoria = _context.Subcategorias
                .FirstOrDefault(subcategoria => subcategoria.Id == id);
            if (subcategoria != null)
            {
                subcategoriaDto.DataModificacao = DateTime.Now;//subcategoria
                _mapper.Map(subcategoriaDto,subcategoria);
                _context.SaveChanges();

                return NoContent();
            }
            return NotFound();
        }
        [HttpDelete("{Id:int}")]
        public IActionResult ExcluiSubcategoria(int Id)
        {
            var subcategoria = _context.Subcategorias.FirstOrDefault(subcategoria => subcategoria.Id == Id);
            if (subcategoria != null)
            {
                _context.Subcategorias.Remove(subcategoria);
                _context.SaveChanges();

                return NoContent();
            }
            return NotFound();
        }
    }
}
