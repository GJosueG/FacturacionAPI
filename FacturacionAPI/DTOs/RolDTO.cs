namespace FacturacionAPI.DTOs
{
    public class RolResponse
    {
        public int RolId { get; set; }

        public string Nombre { get; set; } = null!;
    }

    public class RolRequest
    {
        public int RolId { get; set; }

        public string Nombre { get; set; } = null!;
    }
}
