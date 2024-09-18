namespace FacturacionAPI.Endpoints
{
    public static class Startup
    {
        public static void UseEndpoints(this WebApplication app)
        {
            CategoriaEndpoints.Add(app);
            RolEndpoints.Add(app);
            UsuarioEndpoints.Add(app);
            ProductoEndpoint.Add(app);
            TicketEndpoints.Add(app);
            FacturaEndpoints.Add(app);
            EstadoEndpoints.Add(app);

        }
    }
}
