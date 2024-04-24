using MovieManagement.Payloads.DataRequests.RankCustomerRequest;
using WebXemPhim.Payloads.DataRequests;
using WebXemPhim.Payloads.DataResponses;
using WebXemPhim.Payloads.Responses;

namespace MovieManagement.Services.Interfaces
{
    public interface IRankCustomerServices
    {
        Task<ResponseObject<DataResponsesRankCustomer>> CreateRankCustomer(Requests_CreateRankCustomer request);
        Task<ResponseObject<DataResponsesRankCustomer>> UpdateRankCustomer(Requests_UpdateRankCustomer request);
    }
}
