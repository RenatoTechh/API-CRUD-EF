using ApiCrud.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCrud.Data
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Subcategoria> Subcategorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<CentroDistribuicao> CentrosDistribuicao { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Subcategoria>()
                .HasOne(subcategoria => subcategoria.Categoria)
                .WithMany(categoria => categoria.Subcategorias)
                .HasForeignKey(subcategoria => subcategoria.CategoriaID);

            builder.Entity<Produto>()
                .HasOne(produto => produto.Subcategoria)
                .WithMany(subcategoria => subcategoria.Produtos)
                .HasForeignKey(produto => produto.SubcategoriaId);

            builder.Entity<Produto>()
                .HasOne(produto => produto.CentroDistribuicao)
                .WithMany(centroDistribuicao => centroDistribuicao.Produtos)
                .HasForeignKey(produto => produto.CentroDistribuicaoId);
        }
    }
}
