using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorPeliculas.Client.Repositorios
{
    public class HttpResponseWrapper<T>
    {
        public HttpResponseWrapper(T response, bool error,
            HttpResponseMessage httpResponseMessage)
        {  

        }

        public bool Error { get; set; }
        public T Response { get; set; }
        public HttpResponseMessage httpResponseMessage { get; set; }

        public async Task<string> GetBody()
        {
            return await httpResponseMessage.Content.ReadAsStringAsync();
        }
    }
}
