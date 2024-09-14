using FacturacionAPI.DTOs;

namespace FacturacionAPI.Services.Categorias
{
    public interface ICategoriaServices
    {
        Task<int> PostCategoria(CategoriaRequest categoria);
        Task<List<CategoriaResponse>> GetCategorias();
        Task<CategoriaResponse> GetCategoria(int categoriaId);
        Task<int> PutCategoria(int categoriaId, CategoriaRequest categoria);
        Task<int> DeleteCategoria(int categoriaId);
    }
}
