using AutoMapper;
using FacturacionAPI.DTOs;
using FacturacionAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FacturacionAPI.Services.Facturas
{
    public class FacturaServices : IFacturaServices
    {
        private readonly FacturasDbContext _db;
        private readonly IMapper _mapper;

        public FacturaServices(FacturasDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<int> DeleteFactura(int facturaId)
        {
            var factura = await _db.Facturas.FindAsync(facturaId);
            if (factura == null)
                return -1;

            _db.Facturas.Remove(factura);
            return await _db.SaveChangesAsync();

        }

        public async Task<FacturaResponse> GetFactura(int facturaId)
        {
            var factura = await _db.Facturas.FindAsync(facturaId);
            var  facturaResponse = _mapper.Map<Factura, FacturaResponse>(factura);

            return facturaResponse;

        }

        public async Task<List<FacturaResponse>> GetFacturas()
        {
            var facturas = await _db.Facturas.ToListAsync();
            var facturasList = _mapper.Map<List<Factura>, List<FacturaResponse>>(facturas);

            return facturasList;
        }

        public async Task<int> PostFactura(FacturaRequest factura)
        {
            var facturaRequest = _mapper.Map<FacturaRequest, Factura>(factura);
            await _db.Facturas.AddAsync(facturaRequest);

            return await _db.SaveChangesAsync();
        }

        public async Task<int> PutFactura(int facturaId, FacturaRequest factura)
        {
            var entity = await _db.Facturas.FindAsync(facturaId);
            if (entity == null)
                return -1;

            entity.Descripcion = factura.Descripcion;
            entity.Cantidad = factura.Cantidad;
            entity.Precio = factura.Precio;
            entity.Impuesto = factura.Impuesto;
            entity.FechaEmision = factura.FechaEmision;

            _db.Facturas.Add(entity);

            return await _db.SaveChangesAsync();
        }
    }
}
