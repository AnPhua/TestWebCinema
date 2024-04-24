using WebXemPhim.Payloads.Responses;
using WebXemPhim.Payloads.DataResponses;
using WebXemPhim.Payloads.DataRequests;
using WebXemPhim.Entities;
using WebXemPhim.Handle.HandlePagination;

namespace WebXemPhim.Services.Interfaces
{
    public interface ICinemaServices
    {

        ResponseObject<DataResponsesCinema> CreateCinema(Requests_CreateCinema requests);
        ResponseObject<DataResponsesCinema> UpdateCinema(Requests_UpdateCinema requests);
        string DeleteCinema(int cinemaId);
        Task<PageResult<DataResponsesRoom>> GetListRoomInCinema(int pageSize, int pageNumber);
        Task<PageResult<DataResponsesCinema>> GetListCinema(int pageSize, int pageNumber);
        Task<PageResult<DataResponsesCinema>> GetCinemaByMovie(int movieId, int pageSize, int pageNumber);
    }
}
