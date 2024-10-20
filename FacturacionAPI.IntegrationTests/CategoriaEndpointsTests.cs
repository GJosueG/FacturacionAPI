using FacturacionAPI.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
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


    }
}
