using FacturacionAPI.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FacturacionAPI.IntegrationTests
{
    [TestClass]
    public class CategoriaEndpointsTests
    {
        private static HttpClient _httpClient;
        private static WebApplicationFactory<Program> _factory;
        private static string _token;

        /// <summary>
        /// Configura el entorno de prueba inicializando la API y obteniendo el token JWT.
        /// </summary>

        [ClassInitialize]
        public static async Task ClassInit(TestContext context)
        {
            // Crear instancia de la aplicación en memoria
            _factory = new WebApplicationFactory<Program>();
            _httpClient = _factory.CreateClient();

            // Arrange: Preparar la carga útil para el inicio de sesión
            var loginRequest = new UsuarioRequest { Nombre = "Gerardo", Contrasena = "Gerardo" };

            // Act: Enviar la solicitud de inicio de sesión
            var loginResponse = await _httpClient.PostAsJsonAsync("api/usuarios/login", loginRequest);

            // Verificar que la respuesta fue exitosa
            loginResponse.EnsureSuccessStatusCode();

            // Extraer el token JWT desde la respuesta y eliminar las comillas
            _token = (await loginResponse.Content.ReadAsStringAsync()).Trim('"');
        }

        [TestInitialize]
        public void AgregarTokenALaCabecera()
        {
            // Agregar el token JWT a la cabecera de autorización de todas las solicitudes HTTP
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }

        [TestMethod]
        public async Task ObtenerCategorias_ConTokenValido_RetornaListaDecategorias()
        {
            // Arrange: Pasar autorización a la cabecera
            AgregarTokenALaCabecera();

            // Act: Realizar solicitud para obtener las categorías
            var categorias = await _httpClient.GetFromJsonAsync<List<CategoriaResponse>>("api/categorias");

            // Assert: Verificar que la lista de categorías no sea nula y que tenga elementos
            Assert.IsNotNull(categorias, "La lista de categorías no debería ser nula.");
            Assert.IsTrue(categorias.Count > 0, "La lista de categorías debería contener al menos un elemento.");
        }

        [TestMethod]
        public async Task ObtenerCategoriaPorId_CategoriaExistente_RetornaCategoria()
        {
            // Arrange: Pasar autorización a la cabecera y establecer ID de categoria existente
            AgregarTokenALaCabecera();
            var categoriaId = 1;

            // Act: Realizar solicitud para obtener categoria por ID
            var categoria = await _httpClient.GetFromJsonAsync<CategoriaResponse>($"api/categorias/{categoriaId}");

            // Assert: Verificar que la categoria no sea nula y que tenga el ID correcto
            Assert.IsNotNull(categoria, "La categoria no debería ser nula.");
            Assert.AreEqual(categoriaId, categoria.CategoriaId, "El ID de la categoria devuelta no coincide.");
        }

        [TestMethod]
        public async Task GuardarCategoria_ConNombreValido_RetornaCreated()
        {
            // Arrange: Pasar autorización a la cabecera y preparar la nueva categoría con solo el nombre
            AgregarTokenALaCabecera();
            var nuevaCategoria = new CategoriaRequest { Nombre = "Almacenamiento", EstadoId=1};

            // Act: Realizar solicitud para guardar la categoría
            var response = await _httpClient.PostAsJsonAsync("api/categorias", nuevaCategoria);

            // Assert: Verificar que el código de estado sea 'Created'
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode, "La categoría no se creó correctamente.");
        }

        [TestMethod]
        public async Task GuardarCategoria_NombreDuplicado_RetornaConflict()
        {
            // Arrange: Pasar autorización a la cabecera y preparar la categoría duplicada
            AgregarTokenALaCabecera();
            var newCategoria = new CategoriaRequest { Nombre = "Ropa", EstadoId=1};

            // Act: Realizar solicitud para guardar la categoría con nombre duplicado
            var response = await _httpClient.PostAsJsonAsync("api/categorias", newCategoria);

            // Assert: Verificar el código de estado Conflict
            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode, "Se esperaba un conflicto al intentar crear una categoría duplicada.");
        }

        [TestMethod]
        public async Task ModificarCategoria_CategoriaExistente_RetornaOk()
        {
            // Arrange: Pasar autorización a la cabecera y preparar la categoría modificada, pasando un ID existente
            AgregarTokenALaCabecera();
            var existingCategoria = new CategoriaRequest { Nombre = "Monitores" };
            var categoriaId = 1; // ID de la categoría existente a modificar

            // Act: Realizar solicitud para modificar la categoría existente
            var response = await _httpClient.PutAsJsonAsync($"api/categorias/{categoriaId}", existingCategoria);

            // Assert: Verificar que la respuesta sea OK
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "La categoría no se modificó correctamente.");
        }

        [TestMethod]
        public async Task EliminarCategoria_CategoriaExistente_RetornaNoContent()
        {
            // Arrange: Pasar autorización a la cabecera y establecer el ID de la categoría a eliminar
            AgregarTokenALaCabecera();
            var categoriaId = 14; // ID de la categoría existente a eliminar

            // Act: Realizar solicitud para eliminar la categoría existente
            var response = await _httpClient.DeleteAsync($"api/categorias/{categoriaId}");

            // Assert: Verificar que la respuesta sea NoContent
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode, "La categoría no se eliminó correctamente.");
        }

        [TestMethod]
        public async Task EliminarCategoria_CategoriaNoExistente_RetornaNotFound()
        {
            // Arrange: Pasar autorización a la cabecera y establecer el ID de la categoría inexistente
            AgregarTokenALaCabecera();
            var categoriaId = 25; // ID de una categoría que no existe en la base de datos

            // Act: Realizar solicitud para intentar eliminar una categoría inexistente
            var response = await _httpClient.DeleteAsync($"api/categorias/{categoriaId}");

            // Assert: Verificar que la respuesta sea NotFound
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode,
                "Se esperaba un 404 NotFound al intentar eliminar una categoría inexistente.");
        }

    }
}
