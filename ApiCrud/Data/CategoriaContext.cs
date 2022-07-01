using ApiCrud.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCrud.Data
{
    public class CategoriaContext : DbContext
    {
        public DbSet<Categoria> Categorias { get; set; }
        public CategoriaContext(DbContextOptions<CategoriaContext> opt) : base(opt)
        {

        }
    }
}
