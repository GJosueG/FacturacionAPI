using FacturacionAPI.DTOs;
using FacturacionAPI.Services.Facturas;
using FacturacionAPI.Services.Tickets;
using Microsoft.OpenApi.Models;

namespace FacturacionAPI.Endpoints
{
    public static class TicketEndpoints
    {

        public static void Add(this IEndpointRouteBuilder routes)
        {
            var groups = routes.MapGroup("/api/tickets").WithTags("Tickets");

            groups.MapGet("/", async (ITicketServices ticketServices) =>
            {
                var tickets = await ticketServices.GetTickets();

                //200 OK: La solicitud se realizó correctamente
                // y devuelve las lista de tickets
                return Results.Ok(tickets);
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener Tickets",
                Description = "Muestra una lista de todos los tickets"
            });

            groups.MapGet("/{id}", async (int id, ITicketServices ticketServices) =>
            {
                var ticket = await ticketServices.GetTicket(id);
                if (ticket == null)

                    return Results.NotFound(); //404 Not found: El recurso solicitado no existe.
                else
                    return Results.Ok(ticket); // 200 OK: La solicitud se realizó correctamente y Devuelve el ticket

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener Ticket",
                Description = "Busca un ticket por id"
            }).RequireAuthorization();

            groups.MapPost("/", async (TicketRequest ticket, ITicketServices ticketServices) =>
            {
                if (ticket == null)
                    return Results.BadRequest(); // 400 Bad Request: La solicitud no se pudo procesar, error de formato.

                var id = await ticketServices.PostTicket(ticket);

                //201 Created: El recurso se creó con éxito y devuelve la ubicación del recurso creado.
                return Results.Created($"api/tickets/{id}", ticket);

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Crear Ticket",
                Description = "Crear un nuevo ticket"
            }).RequireAuthorization();

            groups.MapPut("/{id}", async (int id, TicketRequest ticket, ITicketServices ticketServices) =>
            {


                var result = await ticketServices.PutTicket(id, ticket);
                if (result == -1)
                    return Results.NotFound(); //404 Not found: El recurso solicitado no existe.
                else
                    return Results.Ok(result); // 200 OK: La solicitud se realizó correctamente 

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Modificar Ticket",
                Description = "Actualiza un ticket existente"
            }).RequireAuthorization();

            groups.MapDelete("/{id}", async (int id, ITicketServices ticketServices) =>
            {


                var result = await ticketServices.DeleteTicket(id);
                if (result == -1)
                    return Results.NotFound(); //404 Not found: El recurso solicitado no existe.
                else
                    return Results.NoContent(); // 204 Not Content: Recurso Eliminado 



            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Eliminar Ticket",
                Description = "Elimina un ticket existente"
            }).RequireAuthorization();
        }
    }
}
