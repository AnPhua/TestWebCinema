using WebXemPhim.Entities;
using WebXemPhim.Handle.HandlePagination;
using WebXemPhim.Payloads.DataRequests;
using WebXemPhim.Payloads.DataResponses;
using WebXemPhim.Payloads.Responses;

namespace WebXemPhim.Services.Interfaces
{
    public interface ITicketServices
    {
        Task<ResponseObject<DataResponsesTicket>> CreateTicket(int scheduleId, Requests_CreateTicket request);
        Task<ResponseObject<DataResponsesTicket>> UpdateTicket(Requests_UpdateTicket request);
        //Task<List<Ticket>> CreateListTicket(int scheduleId, List<Requests_CreateTicket> requests);
        Task<List<Ticket>> CreateListTicket(int scheduleId);
        Task<DataResponsesTicket[]> GetAllTicketNoPagination(int scheduleId);
        Task<PageResult<DataResponsesSchedule>> GetAllTicketss(int pageSize, int pageNumber);
    }
}
