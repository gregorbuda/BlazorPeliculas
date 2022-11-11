using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorPeliculas.Shared.Entidades
{
    public class CinesPeliculas
    {
        public int PeliculaId { get; set; }
        public int CinesId { get; set; }
        public Cine Cines { get; set; }
        public Pelicula pelicula { get; set; }
    }
}
