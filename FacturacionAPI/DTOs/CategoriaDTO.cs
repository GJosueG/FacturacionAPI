using FacturacionAPI.Models;

namespace FacturacionAPI.DTOs
{
    public class CategoriaResponse
    {
        public int CategoriaId { get; set; }

        public string Nombre { get; set; } = null!;

        public int? EstadoId { get; set; }

        public virtual EstadoResponse? Estado{ get; set; }

    }
    public class CategoriaRequest
    {
        public int CategoriaId { get; set; }

        public string Nombre { get; set; } = null!;

        public int? EstadoId { get; set; }
    }
}
