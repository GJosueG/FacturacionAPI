using AutoMapper;
using FacturacionAPI.DTOs;
using FacturacionAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FacturacionAPI.Services.Roles
{
    public class RolServices : IRolServices
    {
        private readonly FacturasDbContext _db;
        private readonly IMapper _mapper;

        public RolServices(FacturasDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<int> DeleteRol(int rolId)
        {
            var rol = await _db.Roles.FindAsync(rolId);
            if (rol == null)
                return -1;

            _db.Roles.Remove(rol);
            return await _db.SaveChangesAsync();
        }

        public async Task<RolResponse> GetRol(int rolId)
        {
            var rol = await _db.Roles.FindAsync(rolId);
            var rolResponse = _mapper.Map<Rol, RolResponse>(rol);

            return rolResponse;
        }

        public async Task<List<RolResponse>> GetRoles()
        {
            var roles = await _db.Roles.ToListAsync();
            var rolList = _mapper.Map<List<Rol>, List<RolResponse>>(roles);

            return rolList;
        }

        public async Task<int> PostRol(RolRequest rol)
        {
            var rolRequest = _mapper.Map<RolRequest, Rol>(rol);
            await _db.Roles.AddAsync(rolRequest);
            await _db.SaveChangesAsync();
            return rolRequest.RolId;

        }

        public async Task<int> PutRol(int rolId, RolRequest rol)
        {
            var entity = await _db.Roles.FindAsync(rolId);
            if (entity == null)
                return -1;

            entity.Nombre = rol.Nombre;

            _db.Roles.Update(entity);

            return await _db.SaveChangesAsync();
        }
    }
}
