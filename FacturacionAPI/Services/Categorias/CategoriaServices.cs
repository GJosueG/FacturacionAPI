using AutoMapper;
using FacturacionAPI.DTOs;
using FacturacionAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FacturacionAPI.Services.Categorias
{
    public class CategoriaServices
    {
        private readonly FacturasDbContext _db;
        private readonly IMapper _mapper;

        public CategoriaServices(FacturasDbContext facturasDbContext, IMapper mapper)
        {
            _db = facturasDbContext;
            _mapper = mapper;
        }

        public async Task<int> DeleteCategoria(int categoriaId)
        {
            var categoria = await _db.Categorias.FindAsync(categoriaId);

            if (categoria == null)
                return -1;

            _db.Categorias.Remove(categoria);

            return await _db.SaveChangesAsync();
        }

        public async Task<CategoriaResponse> GetCategoria(int categoriaId)
        {
            var categoria = await _db.Categorias.FindAsync(categoriaId);

            var categoriaResponse = _mapper.Map<Categoria, CategoriaResponse>(categoria);
            return categoriaResponse;
        }

        public async Task<List<CategoriaResponse>> GetCategorias()
        {
            var categorias = await _db.Categorias.ToListAsync();
            var categoriasList = _mapper.Map<List<Categoria>, List<CategoriaResponse>>(categorias);

            return categoriasList;
        }

        public async Task<int> PostCategoria(CategoriaRequest categoria)
        {
            var categoriaRequest = _mapper.Map<CategoriaRequest, Categoria>(categoria);
            await _db.Categorias.AddAsync(categoriaRequest);

            return await _db.SaveChangesAsync();
        }

        public async Task<int> PutCategoria(int categoriaId, CategoriaRequest categoria)
        {
            var entity = await _db.Categorias.FindAsync(categoriaId);
            if (entity == null)
                return -1;

            entity.Nombre = categoria.Nombre;

            _db.Categorias.Update(entity);

            return await _db.SaveChangesAsync();
        }
    }
}
