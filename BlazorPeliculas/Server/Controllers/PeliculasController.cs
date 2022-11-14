using AutoMapper;
using BlazorPeliculas.Server.Helpers;
using BlazorPeliculas.Shared.DTOs;
using BlazorPeliculas.Shared.Entidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPeliculas.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public class PeliculasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IAlmacenadorArchivos almacenadorDeArchivos;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        private readonly string contenedor = "peliculas";

        public PeliculasController(ApplicationDbContext context,
        IAlmacenadorArchivos almacenadorDeArchivos,
        IMapper mapper,
        UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.almacenadorDeArchivos = almacenadorDeArchivos;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<HomePageDTO>> Get()
        {
            var limite = 6;

            var peliculasEnCartelera = await context.peliculas
                .Where(x => x.EnCartelera).Take(limite)
                .OrderByDescending(x => x.Lanzamiento)
                .ToListAsync();

            var fechaActual = DateTime.Today;

            var proximosEstrenos = await context.peliculas
                .Where(x => x.Lanzamiento > fechaActual)
                .OrderBy(x => x.Lanzamiento).Take(limite)
                .ToListAsync();

            var response = new HomePageDTO()
            {
                PeliculasEnCartelera = peliculasEnCartelera,
                ProximosEstrenos = proximosEstrenos
            };

            return response;

        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<PeliculaVisualizarDTO>> Get(int id)
        {
            try
            {
                var pelicula = await context.peliculas.Where(x => x.Id == id)
                .Include(x => x.GenerosPelicula).ThenInclude(x => x.genero)
                .Include(x => x.peliculaActor).ThenInclude(x => x.persona)
                .Include(x => x.CinePelicula).ThenInclude(x => x.pelicula)
                .FirstOrDefaultAsync();

                if (pelicula == null) { return NotFound(); }

                // todo: sistema de votacion

                var promedioVotos = 0.0;
                var votoUsuario = 0;

                if (await context.VotosPeliculas.AnyAsync(x => x.PeliculaId == id))
                {
                    promedioVotos = await context.VotosPeliculas.Where(x => x.PeliculaId == id)
                        .AverageAsync(x => x.Voto);

                    if (HttpContext.User.Identity.IsAuthenticated)
                    {
                        var user = await userManager.FindByEmailAsync(HttpContext.User.Identity.Name);
                        var userId = user.Id;

                        var votoUsuarioDB = await context.VotosPeliculas
                            .FirstOrDefaultAsync(x => x.PeliculaId == id && x.UserId == userId);

                        if (votoUsuarioDB != null)
                        {
                            votoUsuario = votoUsuarioDB.Voto;
                        }
                    }
                }

                pelicula.peliculaActor = pelicula.peliculaActor.OrderBy(x => x.Orden).ToList();

                var Cines = pelicula.CinePelicula.ToList();

                List<CineDto> ListCine = new List<CineDto>();

                foreach (var cine in Cines)
                {
                    var listacine = await context.Cines.Select(b =>
                    new CineDto()
                    {
                        Id = b.Id,
                        Nombre = b.Nombre
                    }).Where(x => x.Id == cine.CinesId).FirstOrDefaultAsync();

                    ListCine.Add(listacine);
                };

                var model = new PeliculaVisualizarDTO();
                model.Pelicula = pelicula;
                model.Generos = pelicula.GenerosPelicula.Select(x => x.genero).ToList();
                model.Cines = ListCine;
                model.Actores = pelicula.peliculaActor.Select(x =>
                new Persona
                {
                    Nombre = x.persona.Nombre,
                    Foto = x.persona.Foto,
                    Personaje = x.Personaje,
                    Id = x.PersonaId
                }).ToList();

                model.PromedioVotos = promedioVotos;
                model.VotoUsuario = votoUsuario;
                return model;
            }
            catch (Exception ex)
            {
                var model = new PeliculaVisualizarDTO();
                return model;
            }
        }

        [HttpGet("actualizar/{id}")]
        public async Task<ActionResult<PeliculaActualizacionDTO>> PutGet(int id)
        {
            var peliculaActionResult = await Get(id);
            if (peliculaActionResult.Result is NotFoundResult) { return NotFound(); }

            var peliculaVisualizarDTO = peliculaActionResult.Value;
            var generosSeleccionadosIds = peliculaVisualizarDTO.Generos.Select(x => x.Id).ToList();
            var cineSeleccionadosIds = peliculaVisualizarDTO.Cines.Select(x => x.Id).ToList();
            var generosNoSeleccionados = await context.genero
                .Where(x => !generosSeleccionadosIds.Contains(x.Id))
                .ToListAsync();

            List<CineDto> ListCine = new List<CineDto>();

            var cines = await context.Cines.Select(b =>
            new Cine()
            {
                Id = b.Id,
                Nombre = b.Nombre
            }).ToListAsync();


            foreach (var cine in cines)
            {
                var listacine = await context.Cines.Select(b =>
                new CineDto()
                {
                    Id = b.Id,
                    Nombre = b.Nombre
                }).Where(x => x.Id == cine.Id).FirstOrDefaultAsync();

                ListCine.Add(listacine);
            };


            ListCine = ListCine
            .Where(x => !cineSeleccionadosIds.Contains(x.Id))
            .ToList();

            var model = new PeliculaActualizacionDTO();
            model.Pelicula = peliculaVisualizarDTO.Pelicula;
            model.GenerosNoSeleccionados = generosNoSeleccionados;
            model.GenerosSeleccionados = peliculaVisualizarDTO.Generos;
            model.CinesNoSeleccionados = ListCine;
            model.CinesSeleccionados = peliculaVisualizarDTO.Cines;
            model.Actores = peliculaVisualizarDTO.Actores;
            return model;
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<ActionResult> Put(Pelicula pelicula)
        {
            try
            {
                var peliculaDB = await context.peliculas.FirstOrDefaultAsync(x => x.Id == pelicula.Id);

                if (peliculaDB == null) { return NotFound(); }

                peliculaDB = mapper.Map(pelicula, peliculaDB);

                if (!string.IsNullOrWhiteSpace(pelicula.Poster))
                {
                    var posterImagen = Convert.FromBase64String(pelicula.Poster);
                    peliculaDB.Poster = await almacenadorDeArchivos.EditarArchivo(posterImagen,
                        "jpg", contenedor, peliculaDB.Poster);
                }

                await context.Database.ExecuteSqlInterpolatedAsync($"delete from generoPeliculas WHERE PeliculaId = {pelicula.Id}; delete from peliculaActor where PeliculaId = {pelicula.Id}; delete from CinesPeliculas WHERE PeliculaId = {pelicula.Id}");

                if (pelicula.peliculaActor != null)
                {
                    for (int i = 0; i < pelicula.peliculaActor.Count; i++)
                    {
                        pelicula.peliculaActor[i].Orden = i + 1;
                    }
                }

                peliculaDB.peliculaActor = pelicula.peliculaActor;
                peliculaDB.GenerosPelicula = pelicula.GenerosPelicula;
                peliculaDB.CinePelicula = pelicula.CinePelicula;

                await context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return NoContent();
            }
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<ActionResult<int>> Post(Pelicula pelicula)
        {
            if (!string.IsNullOrWhiteSpace(pelicula.Poster))
            {
                var Posterpelicula = Convert.FromBase64String(pelicula.Poster);
                pelicula.Poster = await almacenadorDeArchivos.GuardarArchivo(Posterpelicula, "jpg", contenedor);
            }

            if (pelicula.peliculaActor != null)
            {
                for (int i = 0; i < pelicula.peliculaActor.Count; i++)
                {
                    pelicula.peliculaActor[i].Orden = i + 1;
                }
            }

            context.Add(pelicula);
            await context.SaveChangesAsync();
            return pelicula.Id;
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.peliculas.AnyAsync(x => x.Id == id);
            if (!existe) { return NotFound(); }
            context.Remove(new Pelicula { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("filtrar")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Pelicula>>> Get([FromQuery] ParametrosBusquedaPeliculas parametrosBusqueda)
        {
            var peliculasQueryable = context.peliculas.AsQueryable();

            if (!string.IsNullOrWhiteSpace(parametrosBusqueda.Titulo))
            {
                peliculasQueryable = peliculasQueryable
                    .Where(x => x.Titulo.ToLower().Contains(parametrosBusqueda.Titulo.ToLower()));
            }

            if (parametrosBusqueda.EnCartelera)
            {
                peliculasQueryable = peliculasQueryable.Where(x => x.EnCartelera);
            }

            if (parametrosBusqueda.Estrenos)
            {
                var hoy = DateTime.Today;
                peliculasQueryable = peliculasQueryable.Where(x => x.Lanzamiento >= hoy);
            }

            if (parametrosBusqueda.GeneroId != 0)
            {
                peliculasQueryable = peliculasQueryable
                    .Where(x => x.GenerosPelicula.Select(y => y.GeneroId)
                    .Contains(parametrosBusqueda.GeneroId));
            }

            if (parametrosBusqueda.MasVotadas)
            {
                var peliculasQueryableLista =
                    from x in peliculasQueryable
                    join y in context.VotosPeliculas on x.Id equals y.PeliculaId into Consulta
                    from pc in Consulta.DefaultIfEmpty()
                    orderby pc.Voto descending
                    select x;


                peliculasQueryable = peliculasQueryableLista.Distinct();
            }


            // TODO: Implementar votacion

            await HttpContext.InsertarParametrosPaginacionEnRespuesta(peliculasQueryable,
                parametrosBusqueda.CantidadRegistros);

            var peliculas = await peliculasQueryable.Paginar(parametrosBusqueda.Paginacion).ToListAsync();

            return peliculas;
        }

        public class ParametrosBusquedaPeliculas
        {
            public int Pagina { get; set; } = 1;
            public int CantidadRegistros { get; set; } = 10;
            public Paginacion Paginacion
            {
                get { return new Paginacion() { Pagina = Pagina, CantidadRegistros = CantidadRegistros }; }
            }
            public string Titulo { get; set; }
            public int PeliculaId { get; set; }
            public int GeneroId { get; set; }
            public bool EnCartelera { get; set; }
            public bool Estrenos { get; set; }
            public bool MasVotadas { get; set; }
        }
    }
}
