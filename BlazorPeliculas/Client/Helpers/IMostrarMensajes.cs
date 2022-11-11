﻿using System.Threading.Tasks;

namespace BlazorPeliculas.Client.Helpers
{
    public interface IMostrarMensajes
    {
        Task MostrarMensajesError(string mensaje);

        Task MostrarMensajesExitoso(string mensaje);
    }
}
