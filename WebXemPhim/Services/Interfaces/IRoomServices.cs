using WebXemPhim.Entities;
using WebXemPhim.Payloads.DataRequests;
using WebXemPhim.Payloads.DataResponses;
using WebXemPhim.Payloads.Responses;

namespace WebXemPhim.Services.Interfaces
{
    public interface IRoomServices
    {
        ResponseObject<DataResponsesRoom> CreateRoom(int cinemaId, Requests_CreateRoom requests);
        List<Room> CreateListRoom(int cinemaId, List<Requests_CreateRoom> requests);
        ResponseObject<DataResponsesRoom> UpdateRoom(Requests_UpdateRoom requests);

        string DeleteRoom(int roomId);
    }
}
