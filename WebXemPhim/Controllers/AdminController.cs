using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieManagement.Payloads.DataRequests.RankCustomerRequest;
using MovieManagement.Services.Implements;
using MovieManagement.Services.Interfaces;
using WebXemPhim.Entities;
using WebXemPhim.Payloads.DataRequests;
using WebXemPhim.Services.Implements;
using WebXemPhim.Services.Interfaces;

namespace WebXemPhim.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly ICinemaServices cinemaServices;
        private readonly IRoomServices roomServices;
        private readonly ISeatServices seatServices;
        private readonly IMovieServices movieServices;
        private readonly IFoodServices foodServices;
        private readonly IScheduleServices scheduleServices;
        private readonly IBannerServices bannerServices;
        private readonly IPromotionServices promotionServices;
        private readonly IRankCustomerServices rankCustomerServices;
        private readonly ITicketServices ticketServices;
        public AdminController(ICinemaServices cinemaServices,
            IScheduleServices scheduleServices, 
            IRoomServices roomServices,
            ISeatServices seatServices,
            IMovieServices movieServices, ITicketServices ticketServices,
            IFoodServices foodServices, 
            IBannerServices bannerServices, 
            IPromotionServices promotionServices, 
            IRankCustomerServices rankCustomerServices)
        {
            this.cinemaServices = cinemaServices;
            this.roomServices = roomServices;
            this.seatServices = seatServices;
            this.movieServices = movieServices;
            this.foodServices = foodServices;
            this.bannerServices = bannerServices;
            this.promotionServices = promotionServices;
            this.rankCustomerServices = rankCustomerServices;
            this.scheduleServices = scheduleServices;
            this.ticketServices = ticketServices;
        }
        [HttpPost("CreateCinema")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateCinema(Requests_CreateCinema requests)
        {
            return Ok(cinemaServices.CreateCinema(requests));
        }
        [HttpPost("UpdateCinema")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateCinema(Requests_UpdateCinema requests)
        {
            return Ok(cinemaServices.UpdateCinema(requests));
        }
        [HttpPut("DeleteCinema")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCinema(int cinemaId)
        {
            return Ok(cinemaServices.DeleteCinema(cinemaId));
        }
        [HttpPost("CreateRoom")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRoom(Requests_CreateRoom requests)
        {
            return Ok(await roomServices.CreateRoom(requests));
        }
        [HttpPost("CreateListRoom")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateListRoom( int cinemaId, List<Requests_CreateRoom> requests)
        {
            return Ok(roomServices.CreateListRoom(cinemaId, requests));
        }
        [HttpPut("UpdateRoom")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateRoom(Requests_UpdateRoom requests)
        {
            return Ok(roomServices.UpdateRoom(requests));
        }
        [HttpPut("DeleteRoom/{roomId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteRoom(int roomId)
        {
            return Ok(roomServices.DeleteRoom(roomId));
        }
        [HttpPost("CreateSeat/{room}")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateSeat(int roomId, Requests_CreateSeat requests)
        {
            return Ok(seatServices.CreateSeat(roomId, requests));
        }
        [HttpPost("CreateListSeat/{roomId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateListSeat(int roomId, [FromBody] List<Requests_CreateSeat> requests)
        {
            return Ok(seatServices.CreateListSeat(roomId, requests));
        }
        //[HttpPost("CreateListTicket/{scheduleId}")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> CreateListTicket(int scheduleId, [FromBody] List<Requests_CreateTicket> requests)
        //{
        //    return Ok(await ticketServices.CreateListTicket(scheduleId, requests));
        //}
        [HttpPost("CreateListTicket/{scheduleId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateListTicket(int scheduleId)
        {
            return Ok(await ticketServices.CreateListTicket(scheduleId));
        }
        [HttpPost("UpdateSeat")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateSeat(int roomId,[FromBody] List<Requests_UpdateSeats> requests)
        {
            return Ok(seatServices.UpdateSeat(roomId,requests));
        }
        [HttpPut("DeleteSeat")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteSeat(int seatId)
        {
            return Ok(seatServices.DeleteSeat(seatId));
        }
        [HttpPost("CreateMovie")]
        [Authorize(Roles = "Admin")]
        [Consumes(contentType: "multipart/form-data")]
        public async Task<IActionResult> CreateMovie([FromForm] Requests_CreateMovie requests)
        {
            return Ok(await movieServices.CreateMovie(requests));
        }
        [HttpPost("CreateBanner")]
        [Authorize(Roles = "Admin,Censor")]
        [Consumes(contentType: "multipart/form-data")]
        public async Task<IActionResult> CreateBanner([FromForm] Requests_CreateBanner requests)
        {
            return Ok(await bannerServices.CreateBanner(requests));
        }
        [HttpPost("CreateMovieType")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateMovieType([FromForm] Requests_CreateMovieType requests)
        {
            return Ok(movieServices.CreateMovieType(requests));
        }
        [HttpPost("CreateRate")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateRate([FromForm] Requests_CreateRate requests)
        {
            return Ok(movieServices.CreateRate(requests));
        }
        [HttpPost("CreateFood")]
        [Authorize(Roles = "Admin")]
        [Consumes(contentType: "multipart/form-data")]
        public async Task<IActionResult> CreateFood([FromForm] Requests_CreateFood requests)
        {
            return Ok(await foodServices.CreateFood(requests));
        }
        
        [HttpPut("DeleteMovieType/{movietypeId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteMovieType(int movietypeId)
        {
            return Ok(movieServices.DeleteMovieType(movietypeId));
        }
        [HttpPut("DeleteRate")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteRate([FromBody] int rateId)
        {
            return Ok(movieServices.DeleteRate(rateId));
        }
        [HttpPut("DeleteFood/{foodId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteFood(int foodId)
        {
            return Ok(foodServices.DeleteFood(foodId));
        }
        [HttpPut("DeleteMovie/{movieId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteMovie( int movieId)
        {
            return Ok(movieServices.DeleteMovie(movieId));
        }
        [HttpPut("UpdateMovie")]
        [Authorize(Roles = "Admin")]
        [Consumes(contentType: "multipart/form-data")]
        public async Task<IActionResult> UpdateMovie([FromForm] Requests_UpdateMovie requests)
        {
            return Ok(await movieServices.UpdateMovie(requests));
        }
        [HttpPut("UpdateMovieHaveString")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateMovieHaveString([FromForm] Requests_UpdateMovieHaveString request)
        {
            return Ok(await movieServices.UpdateMovieHaveString(request));
        }
        [HttpPut("UpdateMovieImageString")]
        [Authorize(Roles = "Admin")]
        [Consumes(contentType: "multipart/form-data")]
        public async Task<IActionResult> UpdateMovieImageString([FromForm] Requests_UpdateMovieImageString requests)
        {
            return Ok(await movieServices.UpdateMovieImageString(requests));
        }
        [HttpPut("UpdateMovieHeroString")]
        [Authorize(Roles = "Admin")]
        [Consumes(contentType: "multipart/form-data")]
        public async Task<IActionResult> UpdateMovieHeroString([FromForm] Requests_UpdateMovieHeroImageString requests)
        {
            return Ok(await movieServices.UpdateMovieHeroImageString(requests));
        }
        [HttpPut("UpdateMovieType")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateMovieType([FromForm] Requests_UpdateMovieType requests)
        {
            return Ok(movieServices.UpdateMovieType(requests));
        }
        [HttpPut("UpdateRate")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateRate([FromForm] Requests_UpdateRate requests)
        {
            return Ok(movieServices.UpdateRate(requests));
        }
        [HttpPut("UpdateFood")]
        [Authorize(Roles = "Admin")]
        [Consumes(contentType: "multipart/form-data")]
        public async Task<IActionResult> UpdateFood([FromForm] Requests_UpdateFood requests)
        {
            return Ok(await foodServices.UpdateFood(requests));
        }
        [HttpPut("UpdateFoodHaveString")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateFoodHaveString([FromForm] Requests_UpdateFoodhavestring requests)
        {
            return Ok(await foodServices.UpdateFoodHaveString(requests));
        }
        [HttpGet("GetAllFoods")]
        public async Task<IActionResult> GetAllFoods(int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await foodServices.GetAllFood(pageSize, pageNumber));
        }
        [HttpGet("GetFoodById/{foodId}")]
        public async Task<IActionResult> GetFoodById([FromRoute] int foodId)
        {
            return Ok(await foodServices.GetFoodById(foodId));
        }
        [HttpDelete("DeleteBanner/{bannerId}")]
        [Authorize(Roles = "Admin, Censor")]
        public async Task<IActionResult> DeleteBanner([FromRoute] int bannerId)
        {
            return Ok(await bannerServices.DeleteBanner(bannerId));
        }
        [HttpGet("GetAllBanners")]
        public async Task<IActionResult> GetAllBanners(int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await bannerServices.GetAllBanners(pageSize, pageNumber));
        }
        [HttpGet("GetAllRooms")]
        public async Task<IActionResult> GetAllRooms(int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await roomServices.GetAllRoom(pageSize, pageNumber));
        }
        [HttpGet("GetRoomById/{roomId}")]
        public async Task<IActionResult> GetRoomById([FromRoute] int roomId)
        {
            return Ok(await roomServices.GetRoomById(roomId));
        }
        [HttpGet("GetSchedulesById/{schId}")]
        public async Task<IActionResult> GetSchedulesById([FromRoute] int schId)
        {
            return Ok(await scheduleServices.GetSchedulesById(schId));
        }
        [HttpGet("GetAllBannersNoPagination")]
        public async Task<IActionResult> GetAllBannersNoPagination()
        {
            return Ok( bannerServices.GetAllBannersNoPagination());
        }
        [HttpGet("GetSeatTypes")]
        public async Task<IActionResult> GetSeatTypes()
        {
            return Ok(seatServices.GetSeatTypes());
        }
        [HttpGet("GetAllMovieTypesNoPagination")]
        public async Task<IActionResult> GetAllMovieTypesNoPagination()
        {
            return Ok(movieServices.GetAllMovieTypeNoPagination());
        }
        [HttpGet("GetAllRoomNoPagination")]
        public async Task<IActionResult> GetAllRoomNoPagination()
        {
            return Ok(roomServices.GetAllRoomNoPagination());
        }
        [HttpGet("GetAllMovieNoPagination")]
        public async Task<IActionResult> GetAllMovieNoPagination()
        {
            return Ok(movieServices.GetAllMovieNoPagination());
        }
        [HttpGet("GetAllTicketBySchedulesId/{schedulesId}")]
        public async Task<IActionResult> GetAllTicketBySchedulesId(int schedulesId)
        {
            return Ok(await ticketServices.GetAllTicketNoPagination(schedulesId));
        }
        [HttpGet("GetAllCinemaNoPagination")]
        public async Task<IActionResult> GetAllCinemaNoPagination()
        {
            return Ok(cinemaServices.GetAllCinemaNoPagination());
        }
        [HttpGet("GetAllRateNoPagination")]
        public async Task<IActionResult> GetAllRateNoPagination()
        {
            return Ok(movieServices.GetAllRateTypeNoPagination());
        }
        [HttpGet("GetBannerById/{bannerId}")]
        public async Task<IActionResult> GetBannerById([FromRoute] int bannerId)
        {
            return Ok(await bannerServices.GetBannerById(bannerId));
        }
        [HttpPut("UpdateBanner")]
        [Authorize(Roles = "Admin, Censor")]
        [Consumes(contentType: "multipart/form-data")]
        public async Task<IActionResult> UpdateBanner([FromForm] Requests_UpdateBanner request)
        {
            return Ok(await bannerServices.UpdateBanner(request));
        }
        [HttpPut("UpdateBannerHaveString")]
        [Authorize(Roles = "Admin, Censor")]
        public async Task<IActionResult> UpdateBannerHaveString([FromForm] Requests_UpdateBannerhavestring request)
        {
            return Ok(await bannerServices.UpdateBannerHaveString(request));
        }
        [HttpPost("CreatePromotion")]
        [Authorize(Roles = "Admin, Censor")]
        public async Task<IActionResult> CreatePromotion(Requests_CreatePromotion request)
        {
            return Ok(await promotionServices.CreatePromotion(request));
        }
        [HttpPut("UpdatePromotion")]
        [Authorize(Roles = "Admin, Censor")]
        public async Task<IActionResult> UpdatePromotion(Requests_UpdatePromotion request)
        {
            return Ok(await promotionServices.UpdatePromotion(request));
        }
        [HttpPost("CreateRankCustomer")]
        [Authorize(Roles = "Admin, Censor")]
        public async Task<IActionResult> CreateRankCustomer(Requests_CreateRankCustomer request)
        {
            return Ok(await rankCustomerServices.CreateRankCustomer(request));
        }
        [HttpPut("UpdateRankCustomer")]
        [Authorize(Roles = "Admin, Censor")]
        public async Task<IActionResult> UpdateRankCustomer(Requests_UpdateRankCustomer request)
        {
            return Ok(await rankCustomerServices.UpdateRankCustomer(request));
        }
    }
}
