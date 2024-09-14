using FacturacionAPI.Models;

namespace FacturacionAPI.DTOs
{
    public class TicketResponse
    {
        public int TicketId { get; set; }

        public string Descripcion { get; set; } = null!;

        public decimal Cantidad { get; set; }

        public decimal Precio { get; set; }

        public decimal Total { get; set; }

        public int? UsuarioId { get; set; }

        public int? EstadoId { get; set; }

        //public virtual EstadoResponse { get; set; }

        //public virtual UsuarioResponse { get; set; }

    }

    public class TicketRequest
    {
        public int TicketId { get; set; }

        public string Descripcion { get; set; } = null!;

        public decimal Cantidad { get; set; }

        public decimal Precio { get; set; }

        public decimal Total { get; set; }

        public int? UsuarioId { get; set; }

        public int? EstadoId { get; set; }
    }
}
