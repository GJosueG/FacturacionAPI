using FacturacionAPI.DTOs;
using FacturacionAPI.Services.Usuarios;
using Microsoft.OpenApi.Models;

namespace FacturacionAPI.Endpoints
{
    public static class UsuarioEndpoints
    {
        public static void Add(this IEndpointRouteBuilder routes)
        {
            var groups = routes.MapGroup("/api/usuarios").WithTags("Usuarios");

            // Obtener una lista de usuarios
            groups.MapGet("/", async (IUsuarioServices usuarioServices) =>
            {
                var usuarios = await usuarioServices.GetUsuarios();
                return Results.Ok(usuarios);
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener Usuarios",
                Description = "Muestra una lista de todos los usuarios."
            });

            // Obtener usuario por id
            groups.MapGet("/{id}", async (int id, IUsuarioServices usuarioServices) =>
            {
                var usuario = await usuarioServices.GetUsuario(id);
                if (usuario == null)
                    return Results.NotFound();
                else
                    return Results.Ok(usuario);
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener Usuario",
                Description = "Busca un usuario por id."
            });

            // Crear usuario
            groups.MapPost("/", async (UsuarioRequest usuario, IUsuarioServices usuarioServices) =>
            {
                if (usuario == null)
                    return Results.BadRequest();

                var id = await usuarioServices.PostUsuario(usuario);
                return Results.Created($"api/usuarios/{id}", usuario);
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Crear Usuario",
                Description = "Crear un nuevo usuario."
            });

            // Modificar usuario
            groups.MapPut("/{id}", async (int id, UsuarioRequest usuario, IUsuarioServices usuarioServices) =>
            {
                var result = await usuarioServices.PutUsuario(id, usuario);
                if (result == -1)
                    return Results.NotFound();
                else
                    return Results.Ok(result);
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Modificar Usuario",
                Description = "Actualiza un usuario existente."
            });

            // Eliminar usuario
            groups.MapDelete("/{id}", async (int id, IUsuarioServices usuarioServices) =>
            {
                var result = await usuarioServices.DeleteUsuario(id);
                if (result == -1)
                    return Results.NotFound(); // 404 Not Found: El recurso solicitado no existe
                else
                    return Results.NoContent(); // 204 No Content: Recurso eliminado
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Eliminar Usuario",
                Description = "Eliminar un usuario existente."
            });
        }
    }
}
