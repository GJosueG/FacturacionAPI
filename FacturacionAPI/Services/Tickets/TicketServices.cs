using AutoMapper;
using FacturacionAPI.DTOs;
using FacturacionAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FacturacionAPI.Services.Tickets
{
    public class TicketServices : ITicketServices
    {
        private readonly FacturasDbContext _db;
        private readonly IMapper _mapper;

        public TicketServices(FacturasDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<int> DeleteTicket(int ticketId)
        {
            var ticket = await _db.Tickets.FindAsync(ticketId);
            if (ticket == null)
                return -1;

            _db.Tickets.Remove(ticket);
            return await _db.SaveChangesAsync();
        }

        public async Task<TicketResponse> GetTicket(int ticketId)
        {
            var ticket = await _db.Tickets.FindAsync(ticketId);
            var ticketResponse = _mapper.Map<Ticket, TicketResponse>(ticket);

            return ticketResponse;
        }

        public async Task<List<TicketResponse>> GetTickets()
        {
            var tickets = await _db.Tickets.ToListAsync();
            var ticketsList = _mapper.Map<List<Ticket>, List<TicketResponse>>(tickets);

            return ticketsList;
        }

        public async Task<int> PostTicket(TicketRequest ticket)
        {
            var ticketRequest = _mapper.Map<TicketRequest, Ticket>(ticket);
            await _db.Tickets.AddAsync(ticketRequest);

            return await _db.SaveChangesAsync();
        }

        public async Task<int> PutTicket(int ticketId, TicketRequest ticket)
        {
            var entity = await _db.Tickets.FindAsync(ticketId);
            if (entity == null)
                return -1;

            entity.Descripcion = ticket.Descripcion;
            entity.Cantidad = ticket.Cantidad;
            entity.Precio = ticket.Precio;
            entity.Total = ticket.Total;

            _db.Tickets.Add(entity);
            return await _db.SaveChangesAsync();
        }
    }
}
