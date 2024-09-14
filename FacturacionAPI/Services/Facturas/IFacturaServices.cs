using FacturacionAPI.DTOs;

namespace FacturacionAPI.Services.Facturas
{
    public interface IFacturaServices
    {
        Task<int> PostFactura(FacturaRequest factura);
        Task<List<FacturaResponse>> GetFacturas();

        Task<FacturaResponse> GetFactura(int facturaId);

        Task<int> PutFactura(int facturaId, FacturaRequest factura);

        Task<int> DeleteFactura(int facturaId);
    }
}
