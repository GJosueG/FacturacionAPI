using FacturacionAPI.DTOs;
using FacturacionAPI.Models;
using FacturacionAPI.Services.Facturas;
using FacturacionAPI.Services.Productos;
using Microsoft.OpenApi.Models;

namespace FacturacionAPI.Endpoints
{
    public static class FacturaEndpoints
    {
        public static void Add(this IEndpointRouteBuilder routes)
        {
            var groups = routes.MapGroup("/api/facturas").WithTags("Facturas");

            groups.MapGet("/", async (IFacturaServices facturaServices) =>
            {
                var facturas = await facturaServices.GetFacturas();

                //200 OK: La solicitud se realizó correctamente
                // y devuelve las lista de facturas
                return Results.Ok(facturas);
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener Facturas",
                Description = "Muestra una lista de todas las facturas"
            });

            groups.MapGet("/{id}", async (int id, IFacturaServices facturaServices) =>
            {
                var fatura = await facturaServices.GetFactura(id);
                if (fatura == null)
                
                    return Results.NotFound(); //404 Not found: El recurso solicitado no existe.
                    else 
                        return Results.Ok(fatura); // 200 OK: La solicitud se realizó correctamente y Devuelve la factura
                
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener Factura",
                Description = "Busca una facutura por id"
            }).RequireAuthorization();

            groups.MapPost("/", async (FacturaRequest factura, IFacturaServices facturaServices) =>
            {
                if (factura == null)
                    return Results.BadRequest(); // 400 Bad Request: La solicitud no se pudo procesar, error de formato.

                var id = await facturaServices.PostFactura(factura);

                //201 Created: El recurso se creó con éxito y devuelve la ubicación del recurso creado.
                return Results.Created($"api/facturas/{id}", factura);

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Crear Factura",
                Description = "Crear una nueva factura"
            }).RequireAuthorization();

            groups.MapPut("/{id}", async (int id, FacturaRequest factura, IFacturaServices facturaServices) =>
            {
               

                var result = await facturaServices.PutFactura(id, factura);
                if (result == -1)
                    return Results.NotFound(); //404 Not found: El recurso solicitado no existe.
                else
                    return Results.Ok(result); // 200 OK: La solicitud se realizó correctamente 

               

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Modificar Factura",
                Description = "Actualiza una factura existente"
            }).RequireAuthorization();

            groups.MapDelete("/{id}", async (int id, IFacturaServices facturaServices) =>
            {


                var result = await facturaServices.DeleteFactura(id);
                if (result == -1)
                    return Results.NotFound(); //404 Not found: El recurso solicitado no existe.
                else
                    return Results.NoContent(); // 204 Not Content: Recurso Eliminado 

             

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Eliminar Factura",
                Description = "Elimina una factura existente"
            }).RequireAuthorization();
        }
    }
}
