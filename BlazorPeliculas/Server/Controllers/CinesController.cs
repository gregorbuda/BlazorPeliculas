using BlazorPeliculas.Shared.DTOs;
using BlazorPeliculas.Shared.Entidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPeliculas.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public class CinesController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public CinesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]

        public async Task<ActionResult<List<Cine>>> Get()
        {

            var cine = await context.Cines.Select(b =>
            new Cine()
            {
                Id = b.Id,
                Nombre = b.Nombre
            }).ToListAsync();

            return cine;
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<List<Cine>>> Get(int id)
        {
            return await context.Cines.Where(x => x.Id == id).ToListAsync();
        }

        [HttpGet("busqueda/{Identidificador}")]
        public async Task<ActionResult<Cine>> GetData(int Identidificador)
        {
            return await context.Cines.Where(x => x.Id == Identidificador).FirstOrDefaultAsync();
        }

        [HttpGet("buscar/{textoBusqueda}")]
        public async Task<ActionResult<List<Cine>>> Get(string textoBusqueda)
        {
            if (string.IsNullOrWhiteSpace(textoBusqueda)) { return new List<Cine>(); }
            textoBusqueda = textoBusqueda.ToLower();
            return await context.Cines
                .Where(x => x.Nombre.ToLower().Contains(textoBusqueda)).ToListAsync();
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<ActionResult<int>> Post(Cine cine)
        {
            context.Add(cine);
            await context.SaveChangesAsync();
            return cine.Id;
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<ActionResult> Put(Cine cine)
        {
            context.Attach(cine).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
