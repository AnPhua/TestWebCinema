﻿using WebXemPhim.Handle.HandlePagination;
using WebXemPhim.Payloads.DataRequests;
using WebXemPhim.Payloads.DataResponses;
using WebXemPhim.Payloads.Responses;

namespace WebXemPhim.Services.Interfaces
{
    public interface IScheduleServices
    {
        Task<ResponseObject<DataResponsesSchedule>> CreateSchedule(Requests_CreateSchedule request);
        Task<ResponseObject<DataResponsesSchedule>> UpdateSchedule(Requests_UpdateSchedule request);
        Task<PageResult<DataResponsesSchedule>> GetSchedulesByMovie(int movieId, int pageSize, int pageNumber);
        Task<PageResult<DataResponsesSchedule>> GetAlls(InputScheduleData input, int pageSize, int pageNumber);
        Task<string> DeleteSchedule(int scheduleId);
        Task<PageResult<DataResponsesSchedule>> GetSchedulesByDay(DateTime startAt, int pageSize, int pageNumber);
    }
}
