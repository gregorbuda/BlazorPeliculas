using BlazorPeliculas.Shared.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BlazorPeliculas.Server
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GeneroPelicula>().HasKey(p => new { p.GeneroId, p.PeliculaId });
            modelBuilder.Entity<PeliculaActor>().HasKey(p => new { p.PeliculaId, p.PersonaId });
            base.OnModelCreating(modelBuilder);
        }

       public DbSet<GeneroPelicula> generoPeliculas { get; set; }
       public DbSet<Pelicula> peliculas { get; set; }
       public DbSet<Genero> genero { get; set; }
       public DbSet<Persona> persona { get; set; }
       public DbSet<PeliculaActor> peliculaActor { get; set; }
    }
}
