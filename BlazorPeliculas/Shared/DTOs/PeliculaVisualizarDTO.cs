﻿using BlazorPeliculas.Shared.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorPeliculas.Shared.DTOs
{
    public class PeliculaVisualizarDTO
    {

        public Pelicula Pelicula { get; set; }
        public List<Genero> Generos { get; set; }
        public List<Persona> Actores { get; set; }
        public List<CineDto> Cines { get; set; }
        public int VotoUsuario { get; set; }
        public double PromedioVotos { get; set; }
    }
}
