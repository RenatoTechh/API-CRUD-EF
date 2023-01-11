using ApiCrud.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCrud.Data
{
    public interface IAppDbContext
    {
        DbSet<Categoria> Categorias { get; set; }
        DbSet<Subcategoria> Subcategorias { get; set; }
        DbSet<Produto> Produtos { get; set; }
        DbSet<CentroDistribuicao> CentrosDistribuicao { get; set; }

        int SaveChanges();
    }
}
