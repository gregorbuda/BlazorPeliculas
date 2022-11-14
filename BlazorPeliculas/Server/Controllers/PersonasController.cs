using AutoMapper;
using BlazorPeliculas.Server.Helpers;
using BlazorPeliculas.Shared.DTOs;
using BlazorPeliculas.Shared.Entidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    public class PersonasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IAlmacenadorArchivos almacenadorDeArchivos;
        private readonly IMapper mapper;
        private readonly string contenedor = "personas";

        public PersonasController(ApplicationDbContext context,
        IAlmacenadorArchivos almacenadorDeArchivos,
        IMapper mapper)
        {
            this.context = context;
            this.almacenadorDeArchivos = almacenadorDeArchivos;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Persona>>> Get([FromQuery] Paginacion paginacion)
        {
            var queryable = context.persona.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacion.CantidadRegistros);
            return await queryable.Paginar(paginacion).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Persona>> Get(int id)
        {
            var persona = await context.persona.FirstOrDefaultAsync(x => x.Id == id);

            if (persona == null) { return NotFound(); }

            return persona;
        }

        [HttpGet("buscar/{textoBusqueda}")]
        public async Task<ActionResult<List<Persona>>> Get(string textoBusqueda)
        {
            if (string.IsNullOrWhiteSpace(textoBusqueda)) { return new List<Persona>(); }
            textoBusqueda = textoBusqueda.ToLower();
            return await context.persona
                .Where(x => x.Nombre.ToLower().Contains(textoBusqueda)).ToListAsync();
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<ActionResult<int>> Post(Persona persona)
        {
            if (!string.IsNullOrWhiteSpace(persona.Foto))
            {
                var fotoPersona = Convert.FromBase64String(persona.Foto);
                persona.Foto = await almacenadorDeArchivos.GuardarArchivo(fotoPersona, "jpg", "personas");
            }

            context.Add(persona);
            await context.SaveChangesAsync();
            return persona.Id;
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<ActionResult> Put(Persona persona)
        {
            var personaDB = await context.persona.FirstOrDefaultAsync(x => x.Id == persona.Id);

            if (personaDB == null) { return NotFound(); }

            personaDB = mapper.Map(persona, personaDB);

            if (!string.IsNullOrWhiteSpace(persona.Foto))
            {
                var fotoImagen = Convert.FromBase64String(persona.Foto);
                personaDB.Foto = await almacenadorDeArchivos.EditarArchivo(fotoImagen,
                    "jpg", contenedor, personaDB.Foto);
            }

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.persona.AnyAsync(x => x.Id == id);
            if (!existe) { return NotFound(); }
            context.Remove(new Persona { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("ver/{PersonaId}")]
        public async Task<ActionResult<PersonaVisualizarDTO>> GetData(int PersonaId)
        {
            var persona = await context.persona.Where(x => x.Id == PersonaId)
            .Include(x => x.peliculaActor).ThenInclude(x => x.persona)
            .FirstOrDefaultAsync();

            if (persona == null) { return NotFound(); }

            persona.peliculaActor = persona.peliculaActor.OrderBy(x => x.Orden).ToList();

            List<Pelicula> Listpelicula = new List<Pelicula>();

            foreach (var peliculas in persona.peliculaActor)
            {
                var Pelicula = await context.peliculas.Where(x => x.Id == peliculas.PeliculaId).FirstOrDefaultAsync();

                Listpelicula.Add(Pelicula);
            }

            var model = new PersonaVisualizarDTO();
            model.persona = persona;
            model.Pelicula = Listpelicula.OrderBy(x => x.Lanzamiento).ToList();

            return model;
        }
    }
}
