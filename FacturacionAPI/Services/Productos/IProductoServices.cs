using FacturacionAPI.DTOs;

namespace FacturacionAPI.Services.Productos
{
    public interface IProductoServices
    {
        Task<int> PostProducto(ProductoRequest producto);
        Task<List<ProductoResponse>> GetProductos();
        Task<ProductoResponse> GetProducto(int productoId);
        Task<int> PutProducto(int productoId, ProductoRequest producto);
        Task<int> DeleteProducto(int productoId);
    }
}
