using FacturacionAPI.DTOs;
using FacturacionAPI.Services.Roles;
using FacturacionAPI.Services.Usuarios;
using Microsoft.OpenApi.Models;

namespace FacturacionAPI.Endpoints
{
    public static class RolEndpoints
    {
        public static void Add(this IEndpointRouteBuilder routes)
        {
            var groups = routes.MapGroup("/api/roles").WithTags("roles");

            // Obtener una lista de roles
            groups.MapGet("/", async (IRolServices rolServices) =>
            {
                var roles = await rolServices.GetRoles();
                return Results.Ok(roles);
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener roles",
                Description = "Muestra una lista de todos los roles."
            });

            // Obtener rol por id
            groups.MapGet("/{id}", async (int id, IRolServices rolServices) =>
            {
                var rol = await rolServices.GetRol(id);
                if (rol == null)
                    return Results.NotFound();
                else
                    return Results.Ok(rol);
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener rol",
                Description = "Busca un rol por id."
            });

            // Crear rol
            groups.MapPost("/", async (RolRequest rol, IRolServices rolServices) =>
            {
                if (rol == null)
                    return Results.BadRequest();

                var id = await rolServices.PostRol(rol);
                return Results.Created($"api/roles/{id}", rol);
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Crear Rol",
                Description = "Crear un nuevo rol."
            });

            // Modificar rol
            groups.MapPut("/{id}", async (int id, RolRequest rol, IRolServices rolServices) =>
            {
                var result = await rolServices.PutRol(id, rol);
                if (result == -1)
                    return Results.NotFound();
                else
                    return Results.Ok(result);
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Modificar rol",
                Description = "Actualiza un rol existente."
            });

            // Eliminar rol
            groups.MapDelete("/{id}", async (int id, IRolServices rolServices) =>
            {
                var result = await rolServices.DeleteRol(id);
                if (result == -1)
                    return Results.NotFound(); // 404 Not Found: El recurso solicitado no existe
                else
                    return Results.NoContent(); // 204 No Content: Recurso eliminado
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Eliminar rol",
                Description = "Eliminar un rol existente."
            });
        }
    }
}
