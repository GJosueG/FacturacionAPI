using FacturacionAPI.DTOs;

namespace FacturacionAPI.Services.Estados
{
    public interface IEstadoServices
    {
        Task<int> PostEstado(EstadoRequest estado);
        Task<List<EstadoResponse>> GetEstados();
        Task<EstadoResponse> GetEstado(int estadoId);
        Task<int> PutEstado(int estadoId, EstadoRequest estado);
        Task<int> DeleteEstado(int estadoId);
    }
}
