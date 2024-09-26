using FacturacionAPI.Models;

namespace FacturacionAPI.DTOs
{
    public class FacturaResponse
    {
        public int FacturaId { get; set; }

        public string Descripcion { get; set; } = null!;

        public decimal Cantidad { get; set; }

        //public decimal Precio { get; set; }

        public decimal Impuesto { get; set; }

        public DateTime FechaEmision { get; set; }

        public int? ProductoId { get; set; }

        public int? UsuarioId { get; set; }

        public int? EstadoId { get; set; }

        public virtual ProductoResponse? Producto { get; set; }

        public virtual EstadoResponse? Estado { get; set; }

        public virtual UsuarioResponse? Usuario { get; set; }

    }

    public class FacturaRequest
    {
        public int FacturaId { get; set; }

        public string Descripcion { get; set; } = null!;

        public decimal Cantidad { get; set; }

        //public decimal Precio { get; set; }

        public decimal Impuesto { get; set; }

        public DateTime FechaEmision { get; set; }

        public int? ProductoId { get; set; }

        public int? UsuarioId { get; set; }

        public int? EstadoId { get; set; }

    }
}
