using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public AdminController(ICinemaServices cinemaServices, IRoomServices roomServices, ISeatServices seatServices,IMovieServices movieServices, IFoodServices foodServices)
        {
            this.cinemaServices = cinemaServices;
            this.roomServices = roomServices;
            this.seatServices = seatServices;
            this.movieServices = movieServices;
            this.foodServices = foodServices;
        }
        [HttpPost("createCinema")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateCinema([FromBody] Requests_CreateCinema requests)
        {
            return Ok(cinemaServices.CreateCinema(requests));
        }
        [HttpPost("updateCinema")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateCinema([FromBody] Requests_UpdateCinema requests)
        {
            return Ok(cinemaServices.UpdateCinema(requests));
        }
        [HttpPut("deleteCinema")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCinema([FromBody] int cinemaId)
        {
            return Ok(cinemaServices.DeleteCinema(cinemaId));
        }
        [HttpPost("createRoom")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateRoom([FromBody] int cinemaId, [FromBody] Requests_CreateRoom requests)
        {
            return Ok(roomServices.CreateRoom(cinemaId, requests));
        }
        [HttpPost("createListRoom")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateListRoom([FromBody] int cinemaId, [FromBody] List<Requests_CreateRoom> requests)
        {
            return Ok(roomServices.CreateListRoom(cinemaId, requests));
        }
        [HttpPost("updateRoom")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateRoom([FromBody] Requests_UpdateRoom requests)
        {
            return Ok(roomServices.UpdateRoom(requests));
        }
        [HttpPut("deleteRoom")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteRoom([FromBody] int roomId)
        {
            return Ok(roomServices.DeleteRoom(roomId));
        }
        [HttpPost("createSeat")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateSeat(int roomId, Requests_CreateSeat requests)
        {
            return Ok(seatServices.CreateSeat(roomId, requests));
        }
        [HttpPost("createListSeat")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateListSeat([FromBody] int roomId, [FromBody] List<Requests_CreateSeat> requests)
        {
            return Ok(seatServices.CreateListSeat(roomId, requests));
        }
        [HttpPost("updateRoom")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateSeat([FromBody] int roomId,[FromBody] List<Requests_UpdateSeats> requests)
        {
            return Ok(seatServices.UpdateSeat(roomId,requests));
        }
        [HttpPut("deleteSeat")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteSeat([FromBody] int seatId)
        {
            return Ok(seatServices.DeleteSeat(seatId));
        }
        [HttpPost("createMovie")]
        [Authorize(Roles = "Admin")]
        [Consumes(contentType: "multipart/form-data")]
        public async Task<IActionResult> CreateMovie([FromForm] Requests_CreateMovie requests)
        {
            return Ok(await movieServices.CreateMovie(requests));
        }
        [HttpPost("createMovieType")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateMovieType([FromForm] Requests_CreateMovieType requests)
        {
            return Ok(movieServices.CreateMovieType(requests));
        }
        [HttpPost("createRate")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateRate([FromForm] Requests_CreateRate requests)
        {
            return Ok(movieServices.CreateRate(requests));
        }
        [HttpPost("createFood")]
        [Authorize(Roles = "Admin")]
        [Consumes(contentType: "multipart/form-data")]
        public async Task<IActionResult> CreateFood([FromForm] Requests_CreateFood requests)
        {
            return Ok(await foodServices.CreateFood(requests));
        }
        [HttpPut("deleteMovie")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteMovie([FromBody] int movieId)
        {
            return Ok(movieServices.DeleteMovie(movieId));
        }
        [HttpPut("deleteMovieType")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteMovieType([FromBody] int movietypeId)
        {
            return Ok(movieServices.DeleteMovieType(movietypeId));
        }
        [HttpPut("deleteRate")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteRate([FromBody] int rateId)
        {
            return Ok(movieServices.DeleteRate(rateId));
        }
        [HttpPut("deleteFood")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteFood([FromBody] int foodId)
        {
            return Ok(foodServices.DeleteFood(foodId));
        }
        [HttpPut("updateMovie")]
        [Authorize(Roles = "Admin")]
        [Consumes(contentType: "multipart/form-data")]
        public async Task<IActionResult> UpdateMovie([FromForm] Requests_UpdateMovie requests)
        {
            return Ok(await movieServices.UpdateMovie(requests));
        }
        [HttpPut("updateMovieType")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateMovieType([FromForm] Requests_UpdateMovieType requests)
        {
            return Ok(movieServices.UpdateMovieType(requests));
        }
        [HttpPut("updateRate")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateRate([FromForm] Requests_UpdateRate requests)
        {
            return Ok(movieServices.UpdateRate(requests));
        }
        [HttpPut("updateFood")]
        [Authorize(Roles = "Admin")]
        [Consumes(contentType: "multipart/form-data")]
        public async Task<IActionResult> UpdateFood([FromForm] Requests_UpdateFood requests)
        {
            return Ok(await foodServices.UpdateFood(requests));
        }
    }
}
