using WebXemPhim.Payloads.Responses;
using WebXemPhim.Payloads.DataResponses;
using WebXemPhim.Payloads.DataRequests;
using WebXemPhim.Entities;
using Azure;

namespace WebXemPhim.Services.Interfaces
{
    public interface IUserServices
    {
        ResponseObject<DataResponsesUser> Register(Requests_Register requests);
        ResponseObject<DataResponsesUser> ConfirmNewAcc(Requests_ConfirmEmail requests);
        DataResponsesToken GenerateAccessToken(User user);
        DataResponsesToken RestartAccessToKen(Requests_RestartToken requests);

        ResponseObject<DataResponsesToken> LoginAcc(Requests_Login requests);
    }
}
