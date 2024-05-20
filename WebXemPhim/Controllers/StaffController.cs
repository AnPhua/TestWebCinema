using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieManagement.Services.Implements;
using WebXemPhim.Handle.HandlePagination;
using WebXemPhim.Payloads.DataRequests;
using WebXemPhim.Payloads.DataResponses;
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
        private readonly IMovieServices _movieServices;
        public StaffController(ICinemaServices _iCinemaService,
            IMovieServices _iMovieService, 
            IScheduleServices _iScheduleService, IBillServices _billService, IMovieServices movieServices, ITicketServices _ticketService)
        {
            this._iCinemaService = _iCinemaService;
            this._iMovieService = _iMovieService;
            this._iScheduleService = _iScheduleService;
            this._billService = _billService;
            _movieServices = movieServices;
            this._ticketService = _ticketService;
        }
        [HttpPost("CreateTicket/{scheduleId}")]
        [Authorize(Roles = "Admin,Censor")]
        public async Task<IActionResult> CreateTicket(int scheduleId,Requests_CreateTicket request)
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
        [HttpGet("GetAllMovie")]
        public async Task<IActionResult> GetAllMovie([FromQuery] InputFilter input, int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _movieServices.GetAllMovie(input, pageSize, pageNumber));
        }
        [HttpGet("GetMovieUnreference")]
        public async Task<IActionResult> GetMovieUnreference([FromQuery] InputDt dt, int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _movieServices.GetMovieUnreference(dt, pageSize, pageNumber));
        }
        [HttpGet("GetMovieShowing")]
        public async Task<IActionResult> GetMovieShowing([FromQuery] InputDt dt, int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _movieServices.GetMovieShowing(dt, pageSize, pageNumber));
        }
        [HttpGet("GetMovieById")]
        public async Task<IActionResult> GetMovieById(int movieId)
        {
            return Ok(await _movieServices.GetMovieById(movieId));
        }
        [HttpGet("GetFeaturedMovies")]
        public async Task<IActionResult> GetFeaturedMovies(int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _movieServices.GetFeaturedMovies(pageSize, pageNumber));
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
        [HttpGet("getSchedulesByMovie/{movieId}")]
        public async Task<IActionResult> GetSchedulesByMovie(int movieId, int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _iScheduleService.GetSchedulesByMovie(movieId, pageSize, pageNumber));
        }
        [HttpGet("GetSchedulesListHour/{movieId}")]
        public async Task<IActionResult> GetSchedulesListHour(int movieId, int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _iScheduleService.GetSchedulesMovielistHours(movieId, pageSize, pageNumber));
        }
        [HttpPut("deleteSchedule/{scheduleId}")]
        public async Task<IActionResult> DeleteSchedule(int scheduleId)
        {
            return Ok(await _iScheduleService.DeleteSchedule(scheduleId));
        }
    }
}
