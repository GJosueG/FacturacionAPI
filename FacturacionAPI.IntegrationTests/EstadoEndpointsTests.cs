﻿using FacturacionAPI.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net.Http.Headers;

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
    }
}
