using FacturacionAPI.DTOs;
using FacturacionAPI.Services.Estados;
using FacturacionAPI.Services.Facturas;
using Microsoft.OpenApi.Models;

namespace FacturacionAPI.Endpoints
{
    public static class EstadoEndpoints
    {
        public static void Add(this IEndpointRouteBuilder routes)
        {
            var groups = routes.MapGroup("/api/estados").WithTags("Estados");
            
            groups.MapGet("/", async (IEstadoServices estadoServices) =>
            {
                var estados = await estadoServices.GetEstados();
                // 200 OK: La solicitud se realizó correctamente
                // y devuelve la lista de estados
                return Results.Ok(estados);
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener Estados",
                Description = "Muestra una lista de todos los estados."
            }).RequireAuthorization();

            groups.MapGet("/{id}", async (int id, IEstadoServices estadoServices) => {
                var estado = await estadoServices.GetEstado(id);

                if (estado == null)
                    return Results.NotFound(); // 404 Not Found: El recurso solicitado no existe
                else
                    return Results.Ok(estado); // 200 OK: La solicitud se realizó correctamente y devuelve el estado
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener Estado",
                Description = "Busca un estado por id."
            }).RequireAuthorization();

            groups.MapPost("/", async (EstadoRequest estado, IEstadoServices estadoServices) => {
                if (estado == null)
                    return Results.BadRequest(); // 400 Bad Request: La solicitud no se pudo procesar, error

                var id = await estadoServices.PostEstado(estado);
                // 201 Created: El recurso se creó con éxito, se devuelve la ubicación del recurso creado
                return Results.Created($"api/estados/{id}", estado);
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Crear Estado",
                Description = "Crear un nuevo estado."
            }).RequireAuthorization();

            groups.MapPut("/{id}", async (int id, EstadoRequest estado, IEstadoServices estadoServices) => {
                var result = await estadoServices.PutEstado(id, estado);
                if (result == -1)
                    return Results.NotFound(); // 404 Not Found: El recurso solicitado no existe
                else
                    return Results.Ok(result); // 200 OK: La solicitud se realizó correctamente
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Modificar Estado",
                Description = "Actualiza un estado existente."
            }).RequireAuthorization();

            groups.MapDelete("/{id}", async (int id, IEstadoServices estadoServices) => {
                var result = await estadoServices.DeleteEstado(id);
                if (result == -1)
                    return Results.NotFound(); // 404 Not Found: El recurso solicitado no existe
                else
                    return Results.NoContent(); // 204 No Content: Recurso eliminado
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Eliminar Estado",
                Description = "Eliminar un estado existente."
            }).RequireAuthorization();

        }
    }
}
