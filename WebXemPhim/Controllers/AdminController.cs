using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public AdminController(ICinemaServices cinemaServices, IRoomServices roomServices, ISeatServices seatServices)
        {
            this.cinemaServices = cinemaServices;
            this.roomServices = roomServices;
            this.seatServices = seatServices;
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
    }
}
