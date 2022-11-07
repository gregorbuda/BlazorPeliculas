using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace BlazorPeliculas.Client.Helpers
{
    public class AlmacenadorArchivosAzStorage : IAlmacenadorArchivos
    {
        private string connectionString;
        
        public AlmacenadorArchivosAzStorage(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("");
        }

        public Task<string> EditarArchivo(byte[] contenido, string extension, string nombreContenedor)
        {
            throw new System.NotImplementedException();
        }

        public Task EliminarArchivos(string ruta, string nombreContenedor)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GuardarArchivo(byte[] contenido, string extension, string nombreContenedor)
        {
            throw new System.NotImplementedException();
        }
    }
}
