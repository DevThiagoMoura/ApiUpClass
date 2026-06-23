using ApiUpClass.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiUpClass.DataContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Curso> Cursos { get; set; }

        public DbSet<Modulo> Modulos { get; set; }

        public DbSet<Aula> Aulas { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<CursoTag> CursosTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CursoTag>()
                .ToTable("cursos_tags")
                .HasKey(x => new { x.CursoId, x.TagId });

            modelBuilder.Entity<CursoTag>()
                .HasOne(x => x.Curso)
                .WithMany(x => x.CursosTags)
                .HasForeignKey(x => x.CursoId);

            modelBuilder.Entity<CursoTag>()
                .HasOne(x => x.Tag)
                .WithMany(x => x.CursosTags)
                .HasForeignKey(x => x.TagId);
        }
    }
}
