using WebXemPhim.Payloads.Responses;
using WebXemPhim.Payloads.DataResponses;
using WebXemPhim.Payloads.DataRequests;

namespace WebXemPhim.Services.Interfaces
{
    public interface IUserServices
    {
        ResponseObject<DataResponsesUser> Register(Requests_Register requests);
        ResponseObject<DataResponsesUser> ConfirmNewAcc(Requests_ConfirmEmail requests);
    }
}
