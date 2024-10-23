using Azure.Identity;
using FacturacionAPI.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Net;

namespace FacturacionAPI.IntegrationTests
{

    [TestClass]
    public class ProductoEndpointTests
    {
        private static HttpClient _httpClient;
        private static WebApplicationFactory<Program> _factory;
        private static string _token;

            ///<summary>
            /// Configurar entorno de prueba inicializando la API y obteniendo el token JWT
            /// </summary>
            /// 

            [ClassInitialize]
           public static async Task ClassInit(TestContext context)
           {
            //Crear instancia de la aplicación en memoria
            _factory = new WebApplicationFactory<Program>();
            //Crear el cliente HTTP
            _httpClient = _factory.CreateClient();


            //Arrange: Preparar la carga util para el inicio de sesión
            var loginRequest = new UsuarioRequest { Nombre = "Gerardo", Contrasena= "Gerardo" };

            //Act: Enviar la solicitud de inicio de sesión
            var loginResponse = await _httpClient.PostAsJsonAsync("api/usuarios/login", loginRequest);

            //Assert: Verificar que el inicio de sesión sea exitoso
            loginResponse.EnsureSuccessStatusCode();
            _token = (await loginResponse.Content.ReadAsStringAsync()).Trim('"');
           }

        /// <summary>
        /// Agregar token de autorización a la cabecera del cliente HTTP
        /// </summary>
        /// 

        [TestInitialize]
        public void AgregarTokenAlaCabecera()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }

        [TestMethod]
        public async Task ObtenerProductos_ConTokenValido_RetornaListaDeproducto()
        {
            // Arrange: Pasar autorización a la cabecera
            AgregarTokenAlaCabecera();

            // Act: Realizar solicitud para obtener los productos
            var productos = await _httpClient.GetFromJsonAsync<List<ProductoResponse>>("api/productos");

            // Assert: Verificar que la lista de productos no sea nula y que tenga elementos
            Assert.IsNotNull(productos, "La lista de productos no debería ser nula.");
            Assert.IsTrue(productos.Count > 0, "La lista de productoss debería contener al menos un elemento.");
        }

        [TestMethod]
        public async Task ObtenerProductoPorId_ProductoExistente_RetornaProducto()
        {
            // Arrange: Pasar autorización a la cabecera y establecer ID de producto existente
            AgregarTokenAlaCabecera();
            var productoId = 5;

            // Act: Realizar solicitud para obtener producto por ID
            var producto = await _httpClient.GetFromJsonAsync<ProductoResponse>($"api/productos/{productoId}");

            // Assert: Verificar que el producto no sea nula y que tenga el ID correcto
            Assert.IsNotNull(producto, "El producto no debería ser nula.");
            Assert.AreEqual(productoId, producto.ProductoId, "El ID de el producto devuelto no coincide.");
        }

        [TestMethod]
        public async Task GuardarProducto_ConDatosValidos_RetornaCreated()
        {
            // Arrange: Pasar autorización a la cabecera y preparar el nuevo producto
            AgregarTokenAlaCabecera();
            var nuevoProducto = new ProductoRequest
            {
                ProductoId = 0,
                Nombre = "Cargador3",
                Precio = 5.00M,
                Stock = 20,
                UsuarioId = 1,
                CategoriaId = 1,
                EstadoId = 1
            };

            // Act: Realizar solicitud para guardar el producto
            var response = await _httpClient.PostAsJsonAsync("api/productos", nuevoProducto);

            // Assert: Verificar el código de estado Created
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode, "El producto no se creó correctamente.");
        }
        [TestMethod]
        public async Task GuardarProducto_NombreDuplicado_RetornaConflict()
        {
            // Arrange: Pasar autorización a la cabecera y preparar el producto duplicado
            AgregarTokenAlaCabecera();
            var nuevoProducto = new ProductoRequest { ProductoId=0, Nombre = "Cargador4", Precio = 5.00m, Stock = 20, UsuarioId = 1, CategoriaId = 1, EstadoId = 1};

            // Act: Realizar solicitud para guardar el producto con nombre duplicado
            var response = await _httpClient.PostAsJsonAsync("api/productos", nuevoProducto);

            // Assert: Verificar el código de estado Conflict
            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode, "Se esperaba un conflicto al intentar crear un producto duplicado.");
        }

        [TestMethod]
        public async Task ModificarProducto_ProductoExistente_RetornaOk()
        {
            // Arrange: Pasar autorización a la cabecera y preparar el producto modificado, pasando un ID existente
            AgregarTokenAlaCabecera();
            var existingProducto = new ProductoRequest { Nombre = "Huawei Y19", Precio = 190.00m, Stock = 20, UsuarioId = 1, CategoriaId = 1, EstadoId = 1 };
            var productoId = 6; // ID del producto existente a modificar

            // Act: Realizar solicitud para modificar el producto existente
            var response = await _httpClient.PutAsJsonAsync($"api/productos/{productoId}", existingProducto);

            // Assert: Verificar que la respuesta sea OK
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "El producto no se modificó correctamente.");
        }

        [TestMethod]
        public async Task EliminarProducto_ProductoExistente_RetornaNoContent()
        {
            // Arrange: Pasar autorización a la cabecera y establecer el ID del producto a eliminar
            AgregarTokenAlaCabecera();
            var productoId = 10; // ID del producto existente a eliminar

            // Act: Realizar solicitud para eliminar el producto existente
            var response = await _httpClient.DeleteAsync($"api/productos/{productoId}");

            // Assert: Verificar que la respuesta sea NoContent
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode, "El producto no se eliminó correctamente.");
        }

        [TestMethod]
        public async Task EliminarProducto_ProductoNoExistente_RetornaNotFound()
        {
            // Arrange: Pasar autorización a la cabecera y establecer el ID del producto inexistente
            AgregarTokenAlaCabecera();
            var productoId = 100; // ID de un producto que no existe en la base de datos

            // Act: Realizar solicitud para intentar eliminar un producto inexistente
            var response = await _httpClient.DeleteAsync($"api/productos/{productoId}");

            // Assert: Verificar que la respuesta sea NotFound
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode,
                "Se esperaba un 404 NotFound al intentar eliminar un producto inexistente.");
        }
    }
}
            

    