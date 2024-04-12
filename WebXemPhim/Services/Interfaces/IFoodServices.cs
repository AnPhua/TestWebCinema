using WebXemPhim.Payloads.DataRequests;
using WebXemPhim.Payloads.DataResponses;
using WebXemPhim.Payloads.Responses;

namespace WebXemPhim.Services.Interfaces
{
    public interface IFoodServices
    {
        Task<ResponseObject<DataResponsesFood>> CreateFood(Requests_CreateFood requests);
        Task<ResponseObject<DataResponsesFood>> UpdateFood(Requests_UpdateFood requests);
        string DeleteFood (int  foodId);
    }
}
