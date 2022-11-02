using BlazorPeliculas.Shared.Entidades;
using System;
using System.Collections.Generic;

namespace BlazorPeliculas.Client.Repositorios
{
    public class Repositorio : IRepositoriocs
    {
        public List<Pelicula> ObtenerPeliculas()
        {
            return new List<Pelicula>()
        {
            new Pelicula() { Titulo = "Nostalgia", Lanzamiento = new DateTime(2022, 7, 2) },
            new Pelicula() { Titulo = "Pelicula", Lanzamiento = new DateTime(2022, 7, 2) },
            new Pelicula() { Titulo = "Pelicula Dos", Lanzamiento = new DateTime(2022, 7, 2) }
        };
        }
    }
}
