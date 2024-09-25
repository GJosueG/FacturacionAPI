using System;
using System.Collections.Generic;

namespace FacturacionAPI.Models;

public partial class Factura
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

    public virtual Producto? Producto {  get; set; }

    public virtual Estado? Estado { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
