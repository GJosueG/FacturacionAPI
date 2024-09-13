using System;
using System.Collections.Generic;

namespace FacturacionAPI.Models;

public partial class Producto
{
    public int ProductoId { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal Precio { get; set; }

    public int Stock { get; set; }

    public int? CategoriaId { get; set; }

    public int? EstadoId { get; set; }

    public virtual Categoria? Categoria { get; set; }

    public virtual Estado? Estado { get; set; }
}
