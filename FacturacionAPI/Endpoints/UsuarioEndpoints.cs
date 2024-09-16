using FacturacionAPI.DTOs;
using FacturacionAPI.Services.Usuarios;

namespace FacturacionAPI.Endpoints
{
    public static class UsuarioEndpoints
    {
        public static void Add(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/usuarios").WithTags("Usuarios");

            //Obtener una lista de usuarios
            group.MapGet("/", async (IUsuarioServices usuarioServices) =>
            {
                var usuarios = await usuarioServices.GetUsuarios();
                return Results.Ok(usuarios);
            }).WhitOpenApi(o=> new OpenApiOperation(o)
            {
                Summary = "Obtener Usuarios",
                Description = "Muestra una lista de todos los usuarios."
            });

            //Obtener usuario por id
            group.MapGet("/{id}", async (int id, IUsuarioServices usuarioServices) =>
            {
                var usuario = await usuarioServices.GetUsuarios(id);
                if (usuario == null)
                    return Results.NotFound();
                else
                return Results.Ok(usuario);
            }).WhitOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener Usuario",
                Description = "Busca un usuario por id."
            });

            //Crear usuario
            group.MapPost("/", async (UsuarioRequest usuario, IUsuarioServices usuarioServices) =>
            {
                if (usuario == null)
                    return Results.BadRequest();

                var id = await usuarioServices.PostUsuario(usuario);
                return Results.Created($"api/usuarios/{id}", usuario);
            }).WhitOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Crear Usuario",
                Description = "Crear un nuevo usuario."
            });

            //Modificar usuario
            group.MapPut("/{id}", async (int id, UsuarioRequest usuario, IUsuarioServices usuarioServices) =>
            {
                var result = await usuarioServices.PutUsuario(id, usuario);
                if (result == -1)
                    return Results.NotFound();
                else
                    return Results.Ok(result);
            }).WhitOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Modificar Usuario",
                Description = "Actualiza un usuario existente."
            });

            //Eliminar usuario
            group.MapDelete("/{id}", async (int id, UsuarioRequest usuario, IUsuarioServices usuarioServices) =>
            {
                var result = await usuarioServices.DeletetUsuario(id);
                if (result == -1)
                    return Results.NotFound();
                else
                    return Results.NotContent;
            }).WhitOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Eliminar Usuario",
                Description = "Eliminar un usuario existente."
            });
        }
    }
}
