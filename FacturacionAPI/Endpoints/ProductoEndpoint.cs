using AutoMapper;
using FacturacionAPI.DTOs;
using FacturacionAPI.Services.Productos;
using Microsoft.OpenApi.Models;

namespace FacturacionAPI.Endpoints
{
    public static class ProductoEndpoint
    {
        public static void Add(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/productos").WithTags("Productos");

            group.MapGet("/", async (IProductoServices productoServices) =>
            {
                var productos = await productoServices.GetProductos();
                //200 Ok: La solicitud se realizo correctamente
                //y devuelve la lista de productos 
                return Results.Ok(productos);
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener Productos",
                Description = "Muestra una lista de todos los productos."
            }).RequireAuthorization();

            group.MapGet("/{id}", async (int id, IProductoServices productoServices) =>
            {
                var producto = await productoServices.GetProducto(id);
                if (producto == null)
                    return Results.NotFound(); //404 Not Found: El recurso solicitado no existe
                else
                    return Results.Ok(producto); //200 Ok: La solicitud se realizo correctamente y devuelve el producto
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener Producto",
                Description = "Busca un producto por id."
            }).RequireAuthorization();


            group.MapPost("/", async (ProductoRequest producto, IProductoServices productoServices) =>{
                if (producto == null)
                    return Results.BadRequest(); // 400 Bad Request: La solicitud no se pudo procesar, error de formato

                var id = await productoServices.PostProducto(producto);
                //201 Created: El recurso se creó con éxito, se devuelve la úbicación del recurso creado
                return Results.Created($"api/productos/{id}", producto);

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Crear Producto",
                Description = "Crear un nuevo producto."
            }).RequireAuthorization();

            group.MapPut("/{id}", async ( int id,ProductoRequest producto, IProductoServices productoServices) =>{
                var result = await productoServices.PutProducto(id,producto);
                if (result == -1)
                    return Results.NotFound(); //404 Not Found: El recurso solicitado no existe
                else
                    return Results.Ok(result); //200 OK: La solicitud se realizo correctamente

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Modificar producto",
                Description = "Actualiza un producto existente."
            }).RequireAuthorization();

            group.MapDelete("/{id}", async (int id, IProductoServices productoServices) => {
                var result = await productoServices.DeleteProducto(id);
                if (result == -1)
                    return Results.NotFound(); //404 Not Found: El recurso solicitado no existe
                else
                    return Results.NoContent(); //204 No Content: Recurso eliminado

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Eliminar producto",
                Description = "Eliminar un producto existente."
            }).RequireAuthorization();
        }

    }
}
