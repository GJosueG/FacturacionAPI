using FacturacionAPI.Models;

namespace FacturacionAPI.DTOs
{
    public class ProductoResponse
    {
        public int ProductoId { get; set; }

        public string Nombre { get; set; } = null!;

        public decimal Precio { get; set; }

        public int Stock { get; set; }

        public int UsuarioId { get; set; }

        public int? CategoriaId { get; set; }

        public int? EstadoId { get; set; }

        public virtual UsuarioResponse Usuario { get; set; }
        public virtual CategoriaResponse? Categoria { get; set; }

        public virtual EstadoResponse? Estado { get; set; }
    }

    public class ProductoRequest
    {
        public int ProductoId { get; set; }

        public string Nombre { get; set; } = null!;

        public decimal Precio { get; set; }

        public int Stock { get; set; }

        public int UsuarioId { get; set; }

        public int? CategoriaId { get; set; }

        public int? EstadoId { get; set; }
      

        //public virtual Usuario? Usuario { get; set; }

        //public virtual Categoria? Categoria { get; set; }
        //public virtual Estado? Estado { get; set; }
    }
}

