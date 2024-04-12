using WebXemPhim.Payloads.Responses;
using WebXemPhim.Payloads.DataResponses;
using WebXemPhim.Payloads.DataRequests;
using WebXemPhim.Entities;
namespace WebXemPhim.Services.Interfaces
{
    public interface ICinemaServices
    {

        ResponseObject<DataResponsesCinema> CreateCinema(Requests_CreateCinema requests);
        ResponseObject<DataResponsesCinema> UpdateCinema(Requests_UpdateCinema requests);
        string DeleteCinema(int cinemaId);
    }
}
