using System;
using System.Collections.Generic;

namespace FacturacionAPI.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public int? EstadoId { get; set; }

    public virtual Estado? Estado { get; set; }

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
