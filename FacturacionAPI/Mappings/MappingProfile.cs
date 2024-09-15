using AutoMapper;
using FacturacionAPI.Models;
using FacturacionAPI.DTOs;
using Azure;

namespace FacturacionAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            //Modelo -> DTO
            CreateMap<Estado, EstadoResponse>();
            CreateMap<Categoria, CategoriaResponse>();
            CreateMap<Producto, ProductoResponse>();
            CreateMap<Rol, RolResponse>();
            CreateMap<Usuario, UsuarioResponse>();
            CreateMap<Ticket, TicketResponse>();
            CreateMap<Factura, FacturaResponse>();

            //DTO -> Modelo
            CreateMap<EstadoRequest, Estado>();
            CreateMap<CategoriaRequest, Categoria>();
            CreateMap<ProductoRequest, Producto>();
            CreateMap<RolRequest, Rol>();
            CreateMap<UsuarioRequest, Usuario>();
            CreateMap<TicketRequest, Ticket>();
            CreateMap<FacturaRequest, Factura>();
        }
    }
}
