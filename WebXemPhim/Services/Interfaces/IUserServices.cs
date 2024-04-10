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
        ResponseObject<DataResponsesToken> RestartAccessToKen(Requests_RestartToken requests);

        ResponseObject<DataResponsesUser> ConfirmEmailLink(Requests_RsPass requests);
        ResponseObject<DataResponsesUser> ResetPasswordconfirmlink(string code, Requests_ChangePass requests1);
        ResponseObject<DataResponsesToken> LoginAcc(Requests_Login requests);
        IQueryable<DataResponsesUser> GetAllInfomation();

        ResponseObject<DataResponsesUser> ChangeYourPassword(int usid,Requests_ChangePass requests);
    }
}
