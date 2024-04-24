﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieManagement.Services.Implements;
using WebXemPhim.Payloads.DataRequests;
using WebXemPhim.Services.Implements;
using WebXemPhim.Services.Interfaces;

namespace WebXemPhim.Controllers
{
    [Route("api/staff")]
    [ApiController]
    public class StaffController : Controller
    {
        private readonly ICinemaServices _iCinemaService;
        private readonly IMovieServices _iMovieService;
        private readonly IScheduleServices _iScheduleService;
        private readonly IBillServices _billService;
        private readonly ITicketServices _ticketService;
        public StaffController(ICinemaServices _iCinemaService,IMovieServices _iMovieService, IScheduleServices _iScheduleService, IBillServices _billService)
        {
            this._iCinemaService = _iCinemaService;
            this._iMovieService = _iMovieService;
            this._iScheduleService = _iScheduleService;
            this._billService = _billService;
        }
        [HttpPost("CreateTicket")]
        [Authorize(Roles = "Admin,Censor")]
        public async Task<IActionResult> CreateTicket(int scheduleId, Requests_CreateTicket request)
        {
            return Ok(await _ticketService.CreateTicket(scheduleId, request));
        }
        [HttpPost("UpdateTicket")]
        [Authorize(Roles = "Admin,Censor")]
        public async Task<IActionResult> UpdateTicket(Requests_UpdateTicket request)
        {
            return Ok(await _ticketService.UpdateTicket(request));
        }
        [HttpGet("getListRoomInCinema")]
        [Authorize(Roles = "Admin,Censor")]
        public async Task<IActionResult> GetListRoomInCinema(int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _iCinemaService.GetListRoomInCinema(pageSize, pageNumber));
        }
        [HttpGet("GetPaymentHistoryByBillId")]
        [Authorize(Roles = "Admin,Censor")]
        public async Task<IActionResult> GetPaymentHistoryByBillId(int billId)
        {
            return Ok(await _billService.GetPaymentHistoryByBillId(billId));
        }
        [HttpGet("GetAllBills")]
        [Authorize(Roles = "Admin,Censor")]
        public async Task<IActionResult> GetAllBills(int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _billService.GetAllBills(pageSize, pageNumber));
        }
        [HttpGet("getFeaturedMovies")]
        [Authorize(Roles = "Admin,Censor")]
        public async Task<IActionResult> GetFeaturedMovies(int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _iMovieService.GetFeaturedMovies(pageSize, pageNumber));
        }
        [HttpGet("SalesStatistics")]
        [Authorize(Roles = "Admin,Censor")]
        public async Task<IActionResult> SalesStatistics([FromQuery] InputStatistic input)
        {
            return Ok(await _billService.SalesStatistics(input));
        }
        [HttpGet("SalesStatisticsFood")]
        [Authorize(Roles = "Admin,Censor")]
        public async Task<IActionResult> SalesStatisticsFood([FromQuery] InputFoodStatistics input)
        {
            return Ok(await _billService.SalesStatisticsFood(input));
        }
        [HttpGet("getSchedulesByMovie")]
        public async Task<IActionResult> GetSchedulesByMovie(int movieId, int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _iScheduleService.GetSchedulesByMovie(movieId, pageSize, pageNumber));
        }
        [HttpPut("deleteSchedule/{scheduleId}")]
        [Authorize(Roles = "Admin,Censor")]
        public async Task<IActionResult> DeleteSchedule([FromRoute] int scheduleId)
        {
            return Ok(await _iScheduleService.DeleteSchedule(scheduleId));
        }
    }
}
