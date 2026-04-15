using ApiUpClass.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiUpClass.DataContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Curso> Cursos { get; set; }
    }
}
