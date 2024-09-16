using FacturacionAPI.DTOs;
using FacturacionAPI.Services.Roles;
using FacturacionAPI.Services.Usuarios;

namespace FacturacionAPI.Endpoints
{
    public static class RolEndpoints
    {
        public static void Add(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/roles").WithTags("roles");

            //Obtener una lista de roles
            group.MapGet("/", async (IRolServices rolServices) =>
            {
                var roles = await rolServices.GetRoles();
                return Results.Ok(roles);
            }).WhitOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener roles",
                Description = "Muestra una lista de todos los roles."
            });

            //Obtener rol por id
            group.MapGet("/{id}", async (int id, IRolServices rolServices) =>
            {
                var rol = await rolServices.GetRoles(id);
                if (roles == null)
                    return Results.NotFound();
                else
                    return Results.Ok(rol);
            }).WhitOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener rol",
                Description = "Busca un rol por id."
            });

            //Crear rol
            group.MapPost("/", async (RolRequest rol, IRolServices rolServices) =>
            {
                if (rol == null)
                    return Results.BadRequest();

                var id = await rolServices.PostRol(rol);
                return Results.Created($"api/roles/{id}", rol);
            }).WhitOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Crear Rol",
                Description = "Crear un nuevo rol."
            });

            //Modificar rol
            group.MapPut("/{id}", async (int id, RolRequest rol, IRolServices rolServices) =>
            {
                var result = await rolServices.PutRol(id, rol);
                if (result == -1)
                    return Results.NotFound();
                else
                    return Results.Ok(result);
            }).WhitOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Modificar rol",
                Description = "Actualiza un rol existente."
            });

            //Eliminar rol
            group.MapDelete("/{id}", async (int id, RolRequest rol, IRolServices rolServices) =>
            {
                var result = await rolServices.DeleteRol(id);
                if (result == -1)
                    return Results.NotFound();
                else
                    return Results.NotContent;
            }).WhitOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Eliminar rol",
                Description = "Eliminar un rol existente."
            });
        }
    }
}
