function pruebaPuntoNetStatic()
{
    DotNet.invokeMethodAsync("BlazorPeliculas.Client", "Obtener")
        .then(resultado => {
            console.log("conteo desde javascript " + resultado);
        });
}
