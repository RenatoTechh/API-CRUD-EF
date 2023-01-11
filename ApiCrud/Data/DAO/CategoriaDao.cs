using ApiCrud.Data.DTOs;
using ApiCrud.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace ApiCrud.Data.DAO
{
    public class CategoriaDao
    {
        IAppDbContext _context;
        IMapper _mapper;
        public CategoriaDao(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // POST
        public Categoria AdicionaCategoria(CreateCategoriaDto categoriaDto)
        {
            var categoria = _mapper.Map<Categoria>(categoriaDto);

            categoria.Status = true;
            categoria.DataCriacao = DateTime.Now;

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return categoria;
        }
        // GET
        public List<Categoria> ListaCategorias()
        {
            return _context.Categorias.OrderBy(categoria => categoria.Nome).ToList();
        }
        // GET
        public List<Categoria> BuscaCategoriaPorNome(string nome)
        {
            return _context.Categorias.Where(categoria => categoria.Nome.ToLower().Contains(nome.ToLower())).ToList();
        }
        // GET
        public Categoria BuscaCategoriaPorId(int id)
        {
            return _context.Categorias.FirstOrDefault(categoria => categoria.CategoriaId == id);
        }
        // GET
        public List<Subcategoria> BuscaSubcategoriaPorIdCategoria(int id)
        {
            return _context.Subcategorias.Where(subcategoria => subcategoria.CategoriaID == id).ToList();
        }
        // PUT
        public void AtualizaCategoria(UpdateCategoriaDto categoriaDto, Categoria categoria)
        {
            _mapper.Map(categoriaDto, categoria);
            _context.SaveChanges();
        }
        // DELETE
        public void DeletaCategoria(Categoria categoria)
        {
            _context.Categorias.Remove(categoria);
            _context.SaveChanges();
        }
    }
}
