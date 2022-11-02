using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace BlazorPeliculas.Client.Helpers
{
    public static class IJSRunTimeExtensionMethods
    {
        public static async ValueTask<bool> Confirm(this IJSRuntime js, string mensaje)
        {
            await js.InvokeVoidAsync("console.log", "prueba de console.log");
            return await js.InvokeAsync<bool>("confirm", mensaje);
        }


    }
}
