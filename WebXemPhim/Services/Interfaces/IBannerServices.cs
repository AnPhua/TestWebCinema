using WebXemPhim.Handle.HandlePagination;
using WebXemPhim.Payloads.DataRequests;
using WebXemPhim.Payloads.DataResponses;
using WebXemPhim.Payloads.Responses;

namespace WebXemPhim.Services.Interfaces
{
    public interface IBannerServices
    {
        Task<ResponseObject<DataResponsesBanner>> CreateBanner(Requests_CreateBanner request);
        Task<ResponseObject<DataResponsesBanner>> UpdateBanner(Requests_UpdateBanner request);
        Task<string> DeleteBanner(int bannerId);
        Task<PageResult<DataResponsesBanner>> GetAllBanners(int pageSize, int pageNumber);
        Task<ResponseObject<DataResponsesBanner>> GetBannerById(int bannerId);
    }
}
