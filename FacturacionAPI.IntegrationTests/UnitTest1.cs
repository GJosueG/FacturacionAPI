using FacturacionAPI.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace FacturacionAPI.IntegrationTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            using var application = new WebApplicationFactory<Program>();

            using var _httpClient = application.CreateClient();

            var userSession = new UsuarioRequest { Nombre = "Gerardo", Contrasena = "Gerardo" };

            var response = await _httpClient.PostAsJsonAsync("api/usuarios/login", userSession);

            if (response.IsSuccessStatusCode)
            {
                // Se asegura de que el resultado sea leído correctamente
                var result = await response.Content.ReadFromJsonAsync<string>();
            }
        }

    }
}