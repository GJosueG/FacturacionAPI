
using AutoMapper;
using FacturacionAPI.DTOs;
using FacturacionAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FacturacionAPI.Services.Estados
{
    public class EstadoServices : IEstadoServices
    {
        private readonly FacturasDbContext _db;
        private readonly IMapper _mapper;

        public EstadoServices(FacturasDbContext facturasDbContext, IMapper mapper)
        {
            _db = facturasDbContext;
            _mapper = mapper;
        }

        public async Task<int> DeleteEstado(int estadoId)
        {
            var estado = await _db.Estados.FindAsync(estadoId);

            if (estado == null)
                return -1;

            _db.Estados.Remove(estado);

            return await _db.SaveChangesAsync();
        }

        public async Task<EstadoResponse> GetEstado(int estadoId)
        {
            var estado = await _db.Estados.FindAsync(estadoId);

            var estadoResponse= _mapper.Map<Estado, EstadoResponse>(estado);
            return estadoResponse;
        }

        public async Task<List<EstadoResponse>> GetEstados()
        {
           var estados = await _db.Estados.ToListAsync();
            var estadosList = _mapper.Map<List<Estado>, List<EstadoResponse>>(estados);

            return estadosList;
        }

        public async Task<int> PostEstado(EstadoRequest estado)
        {
            var estadoRequest= _mapper.Map<EstadoRequest,Estado>(estado);
            await _db.Estados.AddAsync(estadoRequest);

            return await _db.SaveChangesAsync();
        }

        public async Task<int> PutEstado(int estadoId, EstadoRequest estado)
        {
            var entity = await _db.Estados.FindAsync(estadoId);
            if (entity == null)
                return -1;

            entity.Nombre = estado.Nombre;

            _db.Estados.Update(entity);

            return await _db.SaveChangesAsync();
        }
    }
}
