using WebXemPhim.Entities;
using WebXemPhim.Payloads.DataRequests;
using WebXemPhim.Payloads.DataResponses;
using WebXemPhim.Payloads.Responses;

namespace WebXemPhim.Services.Interfaces
{
    public interface ISeatServices
    {
        ResponseObject<DataResponsesSeat> CreateSeat(int roomId, Requests_CreateSeat request);
        List<Seat> CreateListSeat(int roomId, List<Requests_CreateSeat> requests);

        ResponseObject<DataResponsesRoom> UpdateSeat(int roomId, List<Requests_UpdateSeats> requests);
        public string DeleteSeat(int seatId);
    }
}
