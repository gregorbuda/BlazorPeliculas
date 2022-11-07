using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorPeliculas.Client.Pages
{
    public partial class Counter
    {
        [Inject] protected IJSRuntime JS { get; set; }

        protected int currentCount = 0;

        static int currentCountStatic = 0;

        protected async Task IncrementCount()
        {
            currentCount++;
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
