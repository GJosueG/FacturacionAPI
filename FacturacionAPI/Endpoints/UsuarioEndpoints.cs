using FacturacionAPI.DTOs;
using FacturacionAPI.Models;
using FacturacionAPI.Services.Usuarios;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
            }).RequireAuthorization();

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
            }).RequireAuthorization();

            // Crear usuario
            groups.MapPost("/", async (UsuarioRequest usuario, IUsuarioServices usuarioServices) =>
            {
                if (usuario == null)
                    return Results.BadRequest();

                try
                {
                    var id = await usuarioServices.PostUsuario(usuario);
                    return Results.Created($"api/usuarios/{id}", usuario);
                }
                catch (Exception)
                {
                    //409 conflict
                    return Results.Conflict("El Correo ingresado ya está en uso.");
                }

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Crear Usuario",
                Description = "Crear un nuevo usuario."
            }).RequireAuthorization();

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
            }).RequireAuthorization();

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
            }).RequireAuthorization();

            //Inicio de sesión
            groups.MapPost("/login", async (UsuarioRequest usuario, IUsuarioServices usuarioServices, IConfiguration config) => {
                var login = await usuarioServices.Login(usuario);

                if(login is null)
                    return Results.Unauthorized(); //Retorna el estado 401: Unauthorized
                else
                {
                    var jwtSettings = config.GetSection("JwtSetting");
                    var secretkey = jwtSettings.GetValue<string>("secretkey");
                    var issuer = jwtSettings.GetValue<string>("Issuer");
                    var audience = jwtSettings.GetValue<string>("Audience");

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.UTF8.GetBytes(secretkey);

                    var tokenDescriptor = new SecurityTokenDescriptor {
                        Subject = new ClaimsIdentity(new[] {
                            new Claim(ClaimTypes.Name, usuario.Nombre),
                            //new Claim(ClaimTypes.Role, usuario.R)
                        }),
                        Expires = DateTime.UtcNow.AddHours(1),
                        Issuer = issuer,
                        Audience = audience,
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), 
                        SecurityAlgorithms.HmacSha256Signature)
                    };

                    //Crar token, usando parámetros definidos
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    //Convertir el token a una cadena
                    var jwt = tokenHandler.WriteToken(token);

                    return Results.Ok(jwt);                  
                }
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Login Usuario",
                Description = "Generar token para inicio de sesion."
            });
        }
    }
}
