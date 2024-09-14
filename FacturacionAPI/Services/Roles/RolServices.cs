using FacturacionAPI.DTOs;
using FacturacionAPI.Models;

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
            var rol = await_db.Roles.FindAsync(rolId);
            if (rol == null)
                return -1;

            _db.Roles.Remove(rol);
            return await _db.SaveChangesAsync();
        }

        public async Task<RolResponse> GetRol(int rolId)
        {
            var rol = await_db.Roles.FindAsync(rol);
            var rolResponse = _mapper.Map<Roles, RolResponse>(rol);

            return rolResponse;
        }

        public async Task<List<RolResponse>> GetRoles()
        {
            var roles = await_db.Roles.ToListAsync();
            var rolList = _mapper.Map<List<rol>, List<RolResponse>>(roles);

            return rolesList;
        }

        public async Task<int> PostRol(RolRequest rol)
        {
            var rolRequest = _mapper.Map<RolRequest, Rol>(rol);
            await _db.Roles.AddAsync(rolRequest);

            return await _db.SaveChangesAsync();
        }

        public async Task<int> PutRol(int rolId, RolRequest rol)
        {
            var entity = await _db.Roles.FindAsync(rolId);
            if (entity == null)
                return -1;

            entity.Nombre = usuario.Nombre;

            _db.Roles.Update(entity);

            return await _db.SaveChangesAsync();
        }
    }
}
