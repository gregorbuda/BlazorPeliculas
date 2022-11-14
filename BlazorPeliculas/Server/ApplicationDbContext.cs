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

            var roleAdminId = "9548891f-9332-4cef-9fcb-ea21ae8999ff";
            var usuarioAdminId = "95842b29-d857-4c10-825d-852636fd75b5";

            var rolAdmin = new IdentityRole()
            {
                Id = roleAdminId,
                Name = "admin",
                NormalizedName = "admin"
            };

            var hasher = new PasswordHasher<IdentityUser>();

            var usuarioAdmin = new IdentityUser()
            {
                Id = usuarioAdminId,
                Email = "Gregor@hotmail.com",
                UserName = "Gregor@hotmail.com",
                NormalizedUserName = "Gregor@hotmail.com",
                NormalizedEmail = "Gregor@hotmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "f1fe31c4-e16b-48f6-a7c9-505c5499f44d")
            };

            modelBuilder.Entity<IdentityRole>().HasData(usuarioAdmin);

            modelBuilder.Entity<IdentityUserRole<string>>()
              .HasData(new IdentityUserRole<string>() {RoleId = roleAdminId, UserId = usuarioAdminId });

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
