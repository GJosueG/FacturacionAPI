using FacturacionAPI.DTOs;

namespace FacturacionAPI.Services.Roles
{
    public interface IRolServices
    {
        Task<int> PostRol(RolRequest rol);
        Task<List<RolResponse>> GetRoles();
        Task<RolResponse> GetRol(int rolId);
        Task<int> PutRol(int rolId, RolRequest rol);
        Task<int> DeleteRol(int rolId);
    }
}
