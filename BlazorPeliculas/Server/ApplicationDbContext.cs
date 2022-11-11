using BlazorPeliculas.Shared.Entidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorPeliculas.Server
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GeneroPelicula>().HasKey(p => new { p.GeneroId, p.PeliculaId });
            modelBuilder.Entity<PeliculaActor>().HasKey(p => new { p.PeliculaId, p.PersonaId });
            modelBuilder.Entity<CinesPeliculas>().HasKey(p => new { p.PeliculaId, p.CinesId });

            var rolAdmin = new IdentityRole()
            {
                Id = "9548891f-9332-4cef-9fcb-ea21ae8999ff",
                Name = "admin",
                NormalizedName = "admin"
            };

            modelBuilder.Entity<IdentityRole>().HasData(rolAdmin);

            base.OnModelCreating(modelBuilder);
        }

       public DbSet<GeneroPelicula> generoPeliculas { get; set; }
       public DbSet<Pelicula> peliculas { get; set; }
       public DbSet<Genero> genero { get; set; }
       public DbSet<Persona> persona { get; set; }
       public DbSet<PeliculaActor> peliculaActor { get; set; }
       public DbSet<VotoPelicula> VotosPeliculas { get; set; }
       public DbSet<Cine> Cines { get; set; }
       public DbSet<CinesPeliculas> CinesPeliculas { get; set; }
        
    }
}
