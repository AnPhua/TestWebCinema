using WebXemPhim.Handle.HandlePagination;
using WebXemPhim.Payloads.DataRequests;
using WebXemPhim.Payloads.DataResponses;
using WebXemPhim.Payloads.Responses;

namespace WebXemPhim.Services.Interfaces
{
    public interface IFoodServices
    {
        Task<ResponseObject<DataResponsesFood>> CreateFood(Requests_CreateFood requests);
        Task<ResponseObject<DataResponsesFood>> UpdateFood(Requests_UpdateFood requests);
        Task<ResponseObject<DataResponsesFood>> UpdateFoodHaveString(Requests_UpdateFoodhavestring requests);

        Task<PageResult<DataResponsesFood>> GetAllFood(int pageSize, int pageNumber);
        Task<ResponseObject<DataResponsesFood>> GetFoodById(int foodId);
        string DeleteFood (int  foodId);
    }
}
