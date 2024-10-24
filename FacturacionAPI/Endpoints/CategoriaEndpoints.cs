﻿using FacturacionAPI.DTOs;
using FacturacionAPI.Services.Categorias;
using Microsoft.OpenApi.Models;

namespace FacturacionAPI.Endpoints
{
    public static class CategoriaEndpoints
    {

        public static void Add(this IEndpointRouteBuilder routes)
        {
            var groups = routes.MapGroup("/api/categorias").WithTags("Categorias");

            //OBTENER CATEGORIAS
            groups.MapGet("/", async (ICategoriaServices categoriaServices) =>
            {
                var categorias = await categoriaServices.GetCategorias();
                // 200 OK: La solicitud se realizó correctamente
                // y devuelve la lista de categorías
                return Results.Ok(categorias);
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener Categorías",
                Description = "Muestra una lista de todas las categorías."
            }).RequireAuthorization();

            //OBTENER CATEGORIA POR ID
            groups.MapGet("/{id}", async (int id, ICategoriaServices categoriaServices) => {
                var categoria = await categoriaServices.GetCategoria(id);

                if (categoria == null)
                    return Results.NotFound(); // 404 Not Found: El recurso solicitado no existe
                else
                    return Results.Ok(categoria); // 200 OK: La solicitud se realizó correctamente y devuelve la categoría
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener Categoría",
                Description = "Busca una categoría por id."
            }).RequireAuthorization();

            //GUARDAR CATEGORIA
            groups.MapPost("/", async (CategoriaRequest categoria, ICategoriaServices categoriaServices) => {
                if (categoria == null)
                    return Results.BadRequest(); // 400 Bad Request: La solicitud no se pudo procesar, error
             
                try
                {
                    var id = await categoriaServices.PostCategoria(categoria);
                    // 201 Created: El recurso se creó con éxito, se devuelve la ubicación del recurso creado
                    return Results.Created($"api/categorias/{id}", categoria);
                }
                catch (Exception)
                {

                    //409 Conflict
                    return Results.Conflict("La categoria ya existe");
                }

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Crear Categoría",
                Description = "Crear una nueva categoría."
            }).RequireAuthorization();

            //MODIFICAR CATEGORIA
            groups.MapPut("/{id}", async (int id, CategoriaRequest categoria, ICategoriaServices categoriaServices) => {
                var result = await categoriaServices.PutCategoria(id, categoria);
                if (result == -1)
                    return Results.NotFound(); // 404 Not Found: El recurso solicitado no existe
                else
                    return Results.Ok(result); // 200 OK: La solicitud se realizó correctamente
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Modificar Categoría",
                Description = "Actualiza una categoría existente."
            }).RequireAuthorization();

            //ELIMINAR CATEGORIA
            groups.MapDelete("/{id}", async (int id, ICategoriaServices categoriaServices) => {
                var result = await categoriaServices.DeleteCategoria(id);
                if (result == -1)
                    return Results.NotFound(); // 404 Not Found: El recurso solicitado no existe
                else
                    return Results.NoContent(); // 204 No Content: Recurso eliminado
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Eliminar Categoría",
                Description = "Eliminar una categoría existente."
            }).RequireAuthorization();

        }
    }
}
