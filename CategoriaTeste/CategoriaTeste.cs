using ApiCrud.Data.DAO;
using AutoMapper;
using System;
using Xunit;
using Moq;
using ApiCrud.Data;
using ApiCrud.Data.DTOs;
using ApiCrud.Models;
using ApiCrud.Profiles;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CategoriaTeste
{
    public class CategoriaTeste 
    {
        Mock<IAppDbContext> _appDbContext;
        MapperConfiguration _mapperConfig;
        IMapper _mapper;
        Mock<DbSet<Categoria>> _mockSet;
        CategoriaDao _categoria;

        public CategoriaTeste()
        {
            _appDbContext = new Mock<IAppDbContext>();

            //auto mapper configuration
            _mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CategoriaProfile());
            });

            _mapper = _mapperConfig.CreateMapper();

            _mockSet = new Mock<DbSet<Categoria>>();

            _appDbContext.Setup(m => m.Categorias).Returns(_mockSet.Object);

            _categoria = new CategoriaDao(_appDbContext.Object, _mapper);
        }

        [Fact]
        public void CadastrarCategoriaComSucesso()
        {
            var categoriaDto = new CreateCategoriaDto()
            {
                Nome = "CategoriaTeste"
            };

            var cadastroCategoria = _categoria.AdicionaCategoria(categoriaDto);

            _appDbContext.Verify(m => m.Categorias.Add(It.IsAny<Categoria>()), Times.Once());
            _appDbContext.Verify(m => m.SaveChanges(), Times.Once());

            Assert.NotNull(cadastroCategoria);

        }

        [Fact]
        public void RetornarListaDeCategoriasOrdenada()
        {
            //Arrange
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "CategoriaTeste");

            var context = new AppDbContext(optionsBuilder.Options);

            context.Categorias.Add(entity: new Categoria
            {
                Nome = "CategoriaB"
            });
            context.Categorias.Add(entity: new Categoria
            {
                Nome = "CategoriaA"
            });
            context.SaveChanges();

            var listaOrdenada = context.Categorias.OrderBy(categoria => categoria.Nome).ToList();

            var categoria = new CategoriaDao(context, _mapper);

            //Act
            var listaCategorias = categoria.ListaCategorias();

            //Assert
            Assert.True(listaOrdenada.SequenceEqual(listaCategorias));
        }

        [Fact]
        public void RetornaListaDeCategoriasComBaseNoNome()
        {
            //Arrange
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "CategoriaTeste");

            var context = new AppDbContext(optionsBuilder.Options);

            context.Categorias.Add(entity: new Categoria
            {
                Nome = "CategoriaB"
            });
            context.Categorias.Add(entity: new Categoria
            {
                Nome = "CategoriaA"
            });
            context.SaveChanges();

            var nomeCategoria = "Categoria";

            var categoria = new CategoriaDao(context, _mapper);

            var listaCategoriasEsperada = context.Categorias.Where(x => x.Nome.Contains(nomeCategoria)).ToList();

            //Act
            var listaCategorias = categoria.BuscaCategoriaPorNome(nomeCategoria);

            //Assert
            Assert.Equal(listaCategoriasEsperada, listaCategorias);
        }

        [Fact]
        public void RetornaCategoriaComBaseNoId()
        {
            //Arrange
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "CategoriaTeste");

            var context = new AppDbContext(optionsBuilder.Options);

            context.Categorias.Add(entity: new Categoria
            {
                Nome = "CategoriaB"
            });
            context.Categorias.Add(entity: new Categoria
            {
                Nome = "CategoriaA"
            });
            context.SaveChanges();

            var idCategoria = 1;

            var categoriaDao = new CategoriaDao(context, _mapper);

            var categoriaEsperada = context.Categorias.FirstOrDefault(x => x.CategoriaId.Equals(idCategoria));

            //Act
            var categoria = categoriaDao.BuscaCategoriaPorId(idCategoria);

            //Assert
            Assert.Equal(categoriaEsperada, categoria);
        }

        [Fact]
        public void RetornaListaDeSubcategoriaComBaseNoCategoriaId()
        {
            //Arrange
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "CategoriaTeste");

            var context = new AppDbContext(optionsBuilder.Options);

            context.Categorias.Add(entity: new Categoria
            {
                Nome = "CategoriaB"
            });

            context.Subcategorias.Add(entity: new Subcategoria
            {
                Nome = "SubcategoriaA",
                CategoriaID = 1
            });
            context.Subcategorias.Add(entity: new Subcategoria
            {
                Nome = "SubcategoriaB",
                CategoriaID = 1
            });

            context.SaveChanges();

            var idCategoria = 1;

            var categoriaDao = new CategoriaDao(context, _mapper);

            //Act
            var subcategorias = categoriaDao.BuscaSubcategoriaPorIdCategoria(idCategoria);

            //Assert
            Assert.Equal(2, subcategorias.Count);
        }

        [Fact]
        public void AtualizaCategoriaQuandoRecebeDTO()
        {
            //Arrange
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "CategoriaTeste");

            var context = new AppDbContext(optionsBuilder.Options);

            context.Categorias.Add(entity: new Categoria
            {
                Nome = "CategoriaB",
                Status = true
            });
            context.SaveChanges();

            var categoria = context.Categorias.First();

            var categoriaDto = new UpdateCategoriaDto
            {
                Status = false
            };

            var categoriaDao = new CategoriaDao(context, _mapper);

            //Act
            categoriaDao.AtualizaCategoria(categoriaDto, categoria);

            //Assert
            Assert.True(categoria.Status == categoriaDto.Status);
        }

        [Fact]
        public void ExcluiCategoriaQuandoRecebeDTO()
        {
            //Arrange
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "CategoriaTeste");

            var context = new AppDbContext(optionsBuilder.Options);

            context.Categorias.Add(entity: new Categoria
            {
                Nome = "CategoriaB",
                Status = true
            });
            context.SaveChanges();

            var categoria = context.Categorias.FirstOrDefault();

            var categoriaDao = new CategoriaDao(context, _mapper);

            //Act
            categoriaDao.DeletaCategoria(categoria);

            var categorias = context.Categorias.ToList();

            //Assert
            Assert.Empty(categorias);
        }
    }
}
