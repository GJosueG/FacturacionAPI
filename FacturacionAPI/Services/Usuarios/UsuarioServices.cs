using AutoMapper;
using Microsoft.EntityFrameworkCore;
using FacturacionAPI.DTOs;
using FacturacionAPI.Models;

namespace FacturacionAPI.Services.Usuarios
{
    public class UsuarioServices : IUsuarioServices
    {
        private readonly FacturasDbContext _db;
        private readonly IMapper _mapper;

        public UsuarioServices(FacturasDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<int> DeleteUsuario(int usuarioId)
        {
            var usuario = await_db.Usuarios.FindAsync(usuarioId);
            if (usuario == null)
                return -1;

            _db.Usuarios.Remove(usuario);
            return await _db.SaveChangesAsync();
        }

        public async Task<UsuarioResponse> GetUsuario(int usuarioId)
        {
            var usuario = await_db.Usuarios.FindAsync(usuarioId);
            var usuarioResponse = _mapper.Map<Usuario, UsuarioResponse>(usuario);

            return usuarioResponse;
        }

        public async Task<List<UsuarioResponse>> GetUsuarios()
        {
            var usuarios = await_db.Usuarios.ToListAsync();
            var usuariosList = _mapper.Map<List<Usuario>, List<UsuarioResponse>> (usuarios);

            return usuariosList;
        }

        public async Task<int> PostUsuario(UsuarioRequest usuario)
        {
            var usuarioRequest = _mapper.Map<UsuarioRequest, Usuario>(usuario);
            await _db.Usuarios.AddAsync(usuarioRequest);

            return await _db.SaveChangesAsync();
        }

        public async Task <int> PutUsuario(int usuarioId, UsuarioRequest usuario)
        {
            var entity = await _db.Usuarios.FindAsync(usuarioId);
            if (entity == null)
                return -1;

            entity.Nombre = usuario.Nombre;
            entity.Correo = usuario.Correo;
            entity.Contrasena = usuario.Contrasena;

            _db.Usuarios.Update(entity);

            return await _db.SaveChangesAsync();
        }
    }
}
