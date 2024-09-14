using FacturacionAPI.DTOs;

namespace FacturacionAPI.Services.Tickets
{
    public interface ITicketServices
    {
        Task<int> PostTicket(TicketRequest ticket);
        Task<List<TicketResponse>> GetTickets();

        Task<TicketResponse> GetTicket(int ticketId);

        Task<int> PutTicket(int ticketId, TicketRequest ticket);

        Task<int> DeleteTicket(int ticketId);
    }
}
