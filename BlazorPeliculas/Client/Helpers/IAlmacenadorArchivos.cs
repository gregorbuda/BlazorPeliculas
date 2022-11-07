using System.Threading.Tasks;

namespace BlazorPeliculas.Client.Helpers
{
    interface IAlmacenadorArchivos
    {
        Task<string> GuardarArchivo(byte[] contenido, string extension, string nombreContenedor);

        Task EliminarArchivos(string ruta, string nombreContenedor);

        Task<string> EditarArchivo(byte[] contenido, string extension, string nombreContenedor);


    }
}
