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

        private JsonSerializerOptions OpcionesPorDefectoJSON => new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        public async Task<HttpResponseWrapper<T>> Get<T>(string url)
        {
            var responseHttp = await _httpClient.GetAsync(url);

            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await DeserializarRespuesta<T>(responseHttp, OpcionesPorDefectoJSON);

                var respuesta = new HttpResponseWrapper<T>(response, false, responseHttp);
                return respuesta;

            }
            else
            {
                var respuesta= new HttpResponseWrapper<T>(default, true, responseHttp);
                return respuesta;
            }
        }

        public async Task<HttpResponseWrapper<object>> Post<T>(string url,
            T enviar)
        {
            var enviarJSON = JsonSerializer.Serialize(enviar);
            var enviarContent = new StringContent(enviarJSON, Encoding.UTF8, "application/json");
            var responseHttp = await _httpClient.PostAsync(url, enviarContent);
            return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        public async Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url,
    T enviar)
        {
            var enviarJSON = JsonSerializer.Serialize(enviar);
            var enviarContent = new StringContent(enviarJSON, Encoding.UTF8, "application/json");
            var responseHttp = await _httpClient.PostAsync(url, enviarContent);
            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await DeserializarRespuesta<TResponse>(responseHttp, OpcionesPorDefectoJSON);
                return new HttpResponseWrapper<TResponse>(response, false, responseHttp);
            }
            else
            {
                return new HttpResponseWrapper<TResponse>(default, true, responseHttp);
            }
        }

        private async Task<T> DeserializarRespuesta<T>(HttpResponseMessage httpResponseMessage, JsonSerializerOptions jsonSerializerOptions)
        {
            var responseString = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseString, jsonSerializerOptions);
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
