using WebXemPhim.Entities;
using WebXemPhim.Handle.HandlePagination;
using WebXemPhim.Payloads.DataRequests;
using WebXemPhim.Payloads.DataResponses;
using WebXemPhim.Payloads.Responses;

namespace WebXemPhim.Services.Interfaces
{
    public interface IRoomServices
    {
        Task<ResponseObject<DataResponsesRoom>> CreateRoom(Requests_CreateRoom requests);
        List<Room> CreateListRoom(int cinemaId, List<Requests_CreateRoom> requests);
        ResponseObject<DataResponsesRoom> UpdateRoom(Requests_UpdateRoom requests);

        string DeleteRoom(int roomId);
        Task<PageResult<DataResponsesRoom>> GetAllRoom(int pageSize, int pageNumber);
        Task<ResponseObject<DataResponsesRoom>> GetRoomById(int roomId);
        IEnumerable<Room> GetAllRoomNoPagination();
    }
}
