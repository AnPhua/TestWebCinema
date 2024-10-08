﻿using WebXemPhim.Payloads.Responses;
using WebXemPhim.Payloads.DataResponses;
using WebXemPhim.Payloads.DataRequests;
using WebXemPhim.Entities;
using Azure;
using WebXemPhim.Handle.UserName;
using WebXemPhim.Handle.HandlePagination;

namespace WebXemPhim.Services.Interfaces
{
    public interface IAuthServices
    {
        Task<ResponseObject<DataResponsesUser>> Register(Requests_Register requests);
        Task<ResponseObject<DataResponsesUser>> ConfirmNewAcc(Requests_ConfirmEmail requests);
        DataResponsesToken GenerateAccessToken(User user);
        ResponseObject<DataResponsesToken> RestartAccessToKen(Requests_RestartToken requests);
        string GenerateRefreshToken();
        Task<ResponseObject<DataResponsesUser>> ConfirmEmailLink(Requests_RsPass requests);
        Task<ResponseObject<DataResponsesUser>> ResetPasswordconfirmlink(string code, Requests_ChangePass requests1);
        Task<ResponseObject<DataResponsesToken>> LoginAccForStaff(Requests_Login requests);
        Task<ResponseObject<DataResponsesToken>> LoginAccForMember(Requests_Login requests);
        IQueryable<DataResponsesUser> GetAllInfomation();
        Task<ResponseObject<DataResponsesUser>> ChangeDecentralization(Requests_ChangeDecentralization request);
        Task<ResponseObject<DataResponsesUser>> UpdateUserInformation(int userId, Requests_UpdateUserInformation request);
        Task<ResponseObject<DataResponsesUser>> ChangeYourPassword(int usid,Requests_ChangePass requests);
        Task<PageResult<DataResponsesUser>> GetAllUsers(InputUser input, int pageSize, int pageNumber);
        Task<PageResult<DataResponsesUser>> GetListUserByRank(int pageSize, int pageNumber);
        Task<PageResult<DataResponsesUser>> GetUserByName(string name, int pageSize, int pageNumber);

    }
}
