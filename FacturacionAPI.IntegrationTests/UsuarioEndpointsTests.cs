﻿using FacturacionAPI.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using FacturacionAPI.Models;
using System.Net;

namespace FacturacionAPI.IntegrationTests
{
    [TestClass]
    public class UsuarioEndpointsTests
    {
        private static HttpClient _httpClient;
        private static WebApplicationFactory<Program> _factory;
        private static string _token;

        /// <Summary>
        /// Configurar entorno de prueba inicializando la API y obteniendo el token JWT
        /// </Summary>

        [ClassInitialize]
        public static async Task ClassInit(TestContext context)
        {
            //Crear instancia de la aplicación en memoria
            _factory = new WebApplicationFactory<Program>();
            //Crear el cliente HTTP
            _httpClient = _factory.CreateClient();

            //Arrange: Preparar la carga util para el inicio de sesion
            var loginRequest = new UsuarioRequest { Nombre = "Gerardo", Contrasena = "Gerardo" };
            //Act: Enviar la solicitud de inicio de sesion
            var loginResponse = await _httpClient.PostAsJsonAsync("api/usuarios/login", loginRequest);
            //Assert: verificar que el inicio de sesion sea exitoso
            loginResponse.EnsureSuccessStatusCode();
            _token = (await loginResponse.Content.ReadAsStringAsync()).Trim('"');
        }

        /// <Summary>
        /// Agregar token de autorización a la cabecera del cliente HTTP
        /// </Summary>
        [TestInitialize]
        public void AgregarTokenALaCabecera()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }

        [TestMethod]
        public async Task ObtenerUsuarios_ConTokenValido_RetornaListaDeUsuarios()
        {
            //Arrange: Pasar autorizción a la cabecera 
            AgregarTokenALaCabecera();
            //Act: Realizar solicitud para obtener los usuarios
            var usuarios = await _httpClient.GetFromJsonAsync<List<UsuarioResponse>>("api/usuarios");
            //Assert: Verificar que la lista de usuario no sea nula y que tenga elementos
            Assert.IsNotNull(usuarios, "La lista de usuarios no debería ser nula.");
            Assert.IsTrue(usuarios.Count > 0, "La lista de usuarios debería contener al menos un elemento.");
        }

        [TestMethod]
        public async Task ObtenerUsuariosPorId_UsuarioExistente_RetornaUsuario()
        {
            //Arrange: Pasar autorizción a la cabecera  y estables ID de usuario existente
            AgregarTokenALaCabecera();
            var usuarioId = 1;
            //Act: Realizar solicitud para obtener usuario por ID
            var usuario = await _httpClient.GetFromJsonAsync<UsuarioResponse>($"api/usuarios/{usuarioId}");
            //Assert: Verificar que el usuario no sea nulo y que tenga el ID correcto
            Assert.IsNotNull(usuario, "El usuario no debería ser nulo.");
            Assert.AreEqual(usuarioId, usuario.UsuarioId, "El ID del usuario devuelto no coincide.");

        }

        [TestMethod]
        public async Task GuardarUsuario_ConDatosValidos_RetornaCreated()
        {
            //Arrange: Pasar autorización a la cabecera y preparar el uevo usuario
            AgregarTokenALaCabecera();
            var newUsuario = new UsuarioRequest { Nombre = "Beatriz", Contrasena = "Bea12", Correo = "Bea459@gmail.com" };
            //Act: Realizar solicitud para guardar el usuario
            var response = await _httpClient.PostAsJsonAsync("api/usuarios", newUsuario);
            //Assert: Verificar el código de estado created
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode, "El usuario no se creó correctamente");
        }

        [TestMethod]
        public async Task GuardarUsuario_NombreusuarioDuplicado_RetornaConflict()
        {
            //Arrange: Pasar autorización a la cabecera y preparar el correo duplicado
            AgregarTokenALaCabecera();
            var newUsuario = new UsuarioRequest { Nombre = "Alice", Contrasena = "Ali12", Correo = "AliGarcia@gmail.com" };
            //Act: Realizar solicitud para guardar el usuario con correo duplicado
            var response = await _httpClient.PostAsJsonAsync("api/usuarios", newUsuario);
            //Assert: Verificar el código de estado conflict
            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode, "Se esperaba un conflicto al intentar crer un correo duplicado.");
        }

        [TestMethod]
        public async Task ModificarUsuario_UsuarioExistente_RetornaOk()
        {
            //Arrange: Pasar autorización a la cabecera y preparar el usuario modificado, pasando por un ID
            AgregarTokenALaCabecera();
            var existingUsuario = new UsuarioRequest { Nombre = "Brizeyda", Contrasena = "Bris1234", Correo = "Mary@gmail.com" };
            var UsuarioId = 4   ;
            //Act: Realizar solicitud para modificar usuario existente
            var response = await _httpClient.PutAsJsonAsync($"api/usuarios/{UsuarioId}", existingUsuario);
            //Assert: Verifica que la respuesta sea Ok
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "El usuario no se modificó correctamente");
        }

        [TestMethod]
        public async Task EliminarUsuario_UsuarioExistente_RetornaNoContent()
        {
            //Arrange: Pasar autorización a la cabecera, pasando un ID
            AgregarTokenALaCabecera();
            var UsuarioId = 13;
            //Act: Realizar solicitud para eliminar usuario existente
            var response = await _httpClient.DeleteAsync($"api/usuarios/{UsuarioId}");
            //Assert: Verifica que la respuesta sea NoContent
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode, "El usuario no se elimino correctamente");
        }

        [TestMethod]
        public async Task EliminarUsuario_UsuarioNoExistente_RetornaNotFound()
        {
            //Arrange: Pasar autorización a la cabecera, pasando un ID
            AgregarTokenALaCabecera();
            var UsuarioId = 20;
            //Act: Realizar solicitud para eliminar usuario existente
            var response = await _httpClient.DeleteAsync($"api/usuarios/{UsuarioId}");
            //Assert: Verifica que la respuesta sea NoContent
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "Se esperaba un 404 NotFound al intentar eliminar un usuario inexistente");
        }
    }
}

