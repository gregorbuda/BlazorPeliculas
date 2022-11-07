using BlazorPeliculas.Shared.Entidades;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorPeliculas.Client.Repositorios
{
    public class Repositorio : IRepositoriocs
    {
        private readonly HttpClient _httpClient;

        public Repositorio(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<HttpResponseWrapper<object>> Post<T>(string url,
            T enviar)
        {
            var enviarJSON = JsonSerializer.Serialize(enviar);
            var enviarContent = new StringContent(enviarJSON, Encoding.UTF8, "application/json");
            var responseHttp = await _httpClient.PostAsync(url, enviarContent);
            return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        public List<Pelicula> ObtenerPeliculas()
        {
            return new List<Pelicula>()
        {
            new Pelicula() { Titulo = "Nostalgia", 
                Lanzamiento = new DateTime(2022, 7, 2),
                Poster="https://upload.wikimedia.org/wikipedia/commons/c/cb/Andriej_Tarkowski.jpg"},
            new Pelicula() { Titulo = "Pelicula", 
                Lanzamiento = new DateTime(2022, 7, 2),
             Poster="https://upload.wikimedia.org/wikipedia/commons/c/cb/Andriej_Tarkowski.jpg"},
            new Pelicula() { Titulo = "Pelicula Dos",
                Lanzamiento = new DateTime(2022, 7, 2),
             Poster="https://upload.wikimedia.org/wikipedia/commons/c/cb/Andriej_Tarkowski.jpg"}
        };
        }
    }
}
