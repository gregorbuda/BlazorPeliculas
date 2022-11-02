using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorPeliculas.Client.Pages
{
    public partial class Counter
    {
        [Inject]  ServiciosSingleton singleton { get; set; }

        [Inject]  ServiciosTransient transient { get; set; }

        [Inject] protected IJSRuntime JS { get; set; }

        IJSObjectReference modulo;

        protected int currentCount = 0;

        static int currentCountStatic = 0;

        protected async Task IncrementCount()
        {
            modulo = await JS.InvokeAsync<IJSObjectReference>("import", "./js/Counter.js");

            await modulo.InvokeVoidAsync("mostrarAlerta", "hola mundo");

            currentCount++;
            singleton.Valor = currentCount;
            transient.Valor = currentCount;
            currentCountStatic++;
            await JS.InvokeVoidAsync("pruebaPuntoNetStatic");
        }

        [JSInvokable]
        public static Task<int> Obtener()
        {
            return Task.FromResult(currentCountStatic);
        }

    }
}
