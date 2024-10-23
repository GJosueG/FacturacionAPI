using FacturacionAPI.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Net;

namespace FacturacionAPI.IntegrationTests
{
    [TestClass]
    public class EstadoEndpointsTests
    {
        private static HttpClient _httpClient;
        private static WebApplicationFactory<Program> _factory;
        private static string _token;

        ///<summary>
        /// Configurar entorno de preub, inicialilzando la API obteniendo el token JWT
        /// </summary>
        /// 
        [ClassInitialize]
        public static async Task ClassInit(TestContext context)
        {
            //Crear instania de la aplicación en memoria
            _factory = new WebApplicationFactory<Program>();
            //Crear el cliente HTTP
            _httpClient = _factory.CreateClient();

            //Arrange: preparar la carga util para el inicio de sesión
            var loginRequest = new UsuarioRequest { Nombre = "Gerardo", Contrasena = "Gerardo" };
            //Enviar la solicitud de inicio de sesión
            var loginResponse = await _httpClient.PostAsJsonAsync("api/usuarios/login", loginRequest);
            //Assert: verificar que el inicio de sesión sea exitoso
            loginResponse.EnsureSuccessStatusCode();
            _token = (await loginResponse.Content.ReadAsStringAsync()).Trim('"');

        }
        
        /// <summary>
        /// Agregar token de autorización a la cabecera del cliente HTTP
        /// </summary>
        [TestInitialize]
        public void AgregarTokenALaCabecera()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }

        [TestMethod]
        public async Task ObtenerEstados_ConTokenValido_RetornaListaDeEstados()
        {
            //Arrange: Pasar autorización a la cabecera
            AgregarTokenALaCabecera();
            //Act: Realizar solicitud para obtener los estados
            var estados = await _httpClient.GetFromJsonAsync<List<EstadoResponse>>("api/estados/"); 
            //Assert: Verificar que la lista de estados no sea nula y que tenga elementos
            Assert.IsNotNull(estados, "La lista de estados no debería ser nula");
            Assert.IsTrue(estados.Count > 0, "La lista de estados debería contener al menos un elemento");
        }

        [TestMethod]
        public async Task ObtenerEstadoPorId_EstadoExistente_RetornaEstado()
        {
            //Arrange: Pasar autorización a la cabecera y establecer Id de Estado existente
            AgregarTokenALaCabecera();
            var estadoId = 1;
            //Act: Realizar solicitud para obtener estado por Id
            var estado = await _httpClient.GetFromJsonAsync<EstadoResponse>($"api/estados/{estadoId}");
            //Assert: Verificar que el estado no sea nulo y que tenga el Id correcto
            Assert.IsNotNull(estado, "El estado no debería de ser nulo");
            Assert.AreEqual(estadoId, estado.EstadoId, "El Id del estado devuelto no conicide");
        }

        [TestMethod]
        public async Task GuardarEstado_ConDatosValidos_RetornaCreated()
        {
            //Arrange: Pasar autorizacion a la cabecera y preparar el nuevo estado
            AgregarTokenALaCabecera();
            var newEstado = new EstadoRequest { Nombre = "Denegado" };
            //Act: Realizar solicitud para guardar el estado
            var response = await _httpClient.PostAsJsonAsync("api/estados", newEstado);
            //Asert: Verifica el código de estado Created
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode, "El usuario no se creo correctamente");
        }

        [TestMethod]
        public async Task GuardarEstado_NombreDuplicado_RetornaConflict()
        {
            //Arrange: Pasar autorizacion a la cabecera y preparar el estado duplicado
            AgregarTokenALaCabecera();
            var newEstado = new EstadoRequest { Nombre = "Aprobado" };
            //Act: Realizar solicitud para guardar el estado con el nombre de estado duplicado.
            var response = await _httpClient.PostAsJsonAsync("api/estados", newEstado);
            //Asert: Verifica el código de estado Conflict
            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode, "Se esperaba un conflicto al intentar crear un estado duplicado");
        }

        [TestMethod]
        public async Task ModificarEstado_EstadoExistente_RetornaOk()
        {
            //Arrange: Pasar autorización a la cabecera y preparar el estado modificado, pasando un ID
            AgregarTokenALaCabecera();
            var existingEstado = new EstadoRequest { Nombre = "Reprobado" };
            var estadoId = 14;
            //Act: Realizar solicitud para modificar estado existente
            var response = await _httpClient.PutAsJsonAsync($"api/estados/{estadoId}", existingEstado);
            //Asert: Verifica que la respuesta sea OK
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "El usuario no se modificó correctamente");
        }

        [TestMethod]
        public async Task EliminarEstado_EstadoExistente_RetornaNoContent()
        {
            //Arrange: Pasar autorización a la cabecera, pasando un ID
            AgregarTokenALaCabecera();
            var estadoId = 14;
            //Act: Realizar solicitud para eliminar estado existente
            var response = await _httpClient.DeleteAsync($"api/estados/{estadoId}");
            //Asert: Verifica que la respuesta sea NoContent
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode, "El usuario no se elimninó correctamente");
        }

        [TestMethod]
        public async Task EliminarEstado_EstadoNoExistente_RetornaNotFound()
        {
            //Arrange: Pasar autorización a la cabecera, pasando un ID
            AgregarTokenALaCabecera();
            var estadoId = 14;
            //Act: Realizar solicitud para eliminar estado existente
            var response = await _httpClient.DeleteAsync($"api/estados/{estadoId}");
            //Asert: Verifica que la respuesta sea NoContent
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "Se esperaba un 404 NotFound al intentar al intentar eliminar un estado inexistente");
        }
    }
}
