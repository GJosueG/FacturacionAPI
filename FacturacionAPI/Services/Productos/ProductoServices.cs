using AutoMapper;
using FacturacionAPI.DTOs;
using FacturacionAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FacturacionAPI.Services.Productos
{
    public class ProductoServices : IProductoServices
    {
        private readonly FacturasDbContext _db;
        private readonly IMapper _mapper;

        public ProductoServices(FacturasDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<int> DeleteProducto(int productoId)
        {
            var producto = await _db.Productos.FindAsync(productoId);
            if (producto == null)
                return -1;

            _db.Productos.Remove(producto);

            return await _db.SaveChangesAsync();
        }

        public async Task<ProductoResponse> GetProducto(int productoId)
        {
            var producto = await _db.Productos.FindAsync(productoId);
            var productoResponse = _mapper.Map<ProductoResponse>(producto);

            return productoResponse;
        }

        public async Task<List<ProductoResponse>> GetProductos()
        {
            var productos = await _db.Productos.ToListAsync();
            var productoList = _mapper.Map<List<Producto>, List<ProductoResponse>>(productos);

            return productoList;
        }

        public async Task<int> PostProducto(ProductoRequest producto)
        {
            var productoRequest = _mapper.Map<ProductoRequest,Producto>(producto);
            await _db.Productos.AddAsync(productoRequest);
            await _db.SaveChangesAsync();
            return productoRequest.ProductoId;
        }

        public async Task<int> PutProducto(int productoId, ProductoRequest producto)
        {
           var entity = await _db.Productos.FindAsync(productoId);
            if (entity == null)
                return -1;

            entity.Nombre = producto.Nombre;
            entity.Precio = producto.Precio;
            entity.Stock = producto.Stock;
            entity.CategoriaId = producto.CategoriaId;

            _db.Productos.Update(entity);

            return await _db.SaveChangesAsync();
        }
    }
}
