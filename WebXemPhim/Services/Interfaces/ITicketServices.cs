using WebXemPhim.Entities;
using WebXemPhim.Payloads.DataRequests;
using WebXemPhim.Payloads.DataResponses;
using WebXemPhim.Payloads.Responses;

namespace WebXemPhim.Services.Interfaces
{
    public interface ITicketServices
    {
        Task<ResponseObject<DataResponsesTicket>> CreateTicket(int scheduleId, Requests_CreateTicket request);
        Task<ResponseObject<DataResponsesTicket>> UpdateTicket(Requests_UpdateTicket request);
        List<Ticket> CreateListTicket(int scheduleId, List<Requests_CreateTicket> requests);
        Task<DataResponsesTicket[]> GetAllTicketNoPagination(int scheduleId);
    }
}
