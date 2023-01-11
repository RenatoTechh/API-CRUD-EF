using ApiCrud.Data.DTOs;
using ApiCrud.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiCrud.Data.DAO
{
    public class SubcategoriaDao
    {
        AppDbContext _context;
        IMapper _mapper;
        public SubcategoriaDao(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        //  GET
        public Categoria BuscaCategoriaIdSubcategoria(CreateSubcategoriaDto subcategoriaDto)
        {
            // Verifica o Id da Categoria com a chave estrangeira da subcategoria
            return _context.Categorias.FirstOrDefault(categoria => categoria.CategoriaId == subcategoriaDto.CategoriaID);
        }
        // POST
        public void AddSubcategoria(Subcategoria subcategoria)
        {
            _context.Subcategorias.Add(subcategoria);
            _context.SaveChanges();
        }
        //  GET
        public List<Subcategoria> BuscaSubcategoriaPorNome(string nome)
        {
            // Realiza a busca no banco através do nome
            return _context.Subcategorias.Where(subcategoria => subcategoria.Nome.ToLower() == nome.ToLower()).ToList();
        }
        //  GET
        public List<Subcategoria> RetornaTodasSubcategorias()
        {
            return _context.Subcategorias.ToList();
        }
        //  GET
        public List<Subcategoria> RetornaSubcategoriasStatus(int skip, int take, bool? status)
        {
            return _context.Subcategorias.Where(s => s.Status == status).Skip(skip).Take(take).ToList();
        }
        //  GET
        public List<Subcategoria> RetornaSubcategoriasStatusENome(int skip, int take, string nome, bool? status)
        {
            return _context.Subcategorias.Where(s => s.Status == status && s.Nome.ToLower().Contains(nome.ToLower())).Skip(skip).Take(take).ToList();
        }
        //  GET
        public Subcategoria RetornaSubcategoriaPorId(int id)
        {
            return _context.Subcategorias.FirstOrDefault(subcategoria => subcategoria.Id == id);
        }
        // PUT
        public void AlteraStatus(UpdateSubcategoriaDto subcategoriaDto, Subcategoria subcategoria)
        {
            subcategoria.DataModificacao = DateTime.Now;
            _mapper.Map(subcategoriaDto, subcategoria);
            _context.SaveChanges();
        }
        // DELETE
        public void RemoveSubcategoria(Subcategoria subcategoria)
        {
            _context.Subcategorias.Remove(subcategoria);
            _context.SaveChanges();
        }
    }
}
