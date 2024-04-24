using WebXemPhim.Handle.HandlePagination;
using WebXemPhim.Payloads.DataRequests;
using WebXemPhim.Payloads.DataResponses;
using WebXemPhim.Payloads.Responses;

namespace MovieManagement.Services.Interfaces
{
    public interface IPromotionServices
    {
        Task<ResponseObject<DataRepsonsesPromotion>> CreatePromotion(Requests_CreatePromotion request);
        Task<ResponseObject<DataRepsonsesPromotion>> UpdatePromotion(Requests_UpdatePromotion request);
        Task<PageResult<DataRepsonsesPromotion>> GetAllPromotions(int pageSize, int pageNumber);
    }
}
