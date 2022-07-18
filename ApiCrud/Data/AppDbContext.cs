using ApiCrud.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCrud.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Subcategoria> Subcategorias { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Subcategoria>()
                .HasOne(subcategoria => subcategoria.Categoria)
                .WithMany(categoria => categoria.Subcategorias)
                .HasForeignKey(subcategoria => subcategoria.CategoriaID);
        }
    }
}
