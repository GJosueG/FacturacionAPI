namespace FacturacionAPI.DTOs
{
    public class EstadoResponse
    {
        public int EstadoId { get; set; }

        public string Nombre { get; set; } = null!;

    }

    public class EstadoRequest
    {
        //public int EstadoId { get; set; }

        public string Nombre { get; set; } = null!;

    }
}
