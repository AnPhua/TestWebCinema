using Azure.Core;
using WebXemPhim.Entities;
using WebXemPhim.Payloads.DataRequests;
using WebXemPhim.Payloads.DataResponses;
using WebXemPhim.Payloads.Responses;
using WebXemPhim.Payloads.Converters;
using WebXemPhim.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WebXemPhim.Services.Implements
{
    public class CinemaServices : BaseServices, ICinemaServices
    {
        private readonly ResponseObject<DataResponsesCinema> responsescinema;
        private readonly CinemaConverter cinemaConverter;
        private readonly IRoomServices _iRoomServices;
        public CinemaServices(IRoomServices iRoomServices)
        {
            responsescinema = new ResponseObject<DataResponsesCinema>();
            _iRoomServices = iRoomServices;
        }
        public ResponseObject<DataResponsesCinema> CreateCinema(Requests_CreateCinema requests)
        {
            if (string.IsNullOrWhiteSpace(requests.Address) || 
                string.IsNullOrWhiteSpace(requests.Description) || 
                string.IsNullOrWhiteSpace(requests.Code) || 
                string.IsNullOrWhiteSpace(requests.NameOfCinema))
            {
                return responsescinema.ResponseFail(StatusCodes.Status400BadRequest, "Thông Tin Chưa Điền Đầy Đủ!!", null);
            }
            var cinema = new Cinema
            {
                Address = requests.Address,
                Code = requests.Code,
                Description = requests.Description,
                NameOfCinema = requests.NameOfCinema,
                Room = null,
                IsActive = true,
            };
            _appDbContext.Cinemas.Add(cinema);
            _appDbContext.SaveChanges();
            cinema.Room = requests.Request_CreateRooms == null ? null :  _iRoomServices.CreateListRoom(cinema.Id, requests.Request_CreateRooms);         
            _appDbContext.Cinemas.Update(cinema);
            _appDbContext.SaveChanges();
            return responsescinema.ResponseSucess("Thêm Rạp Thành Công", cinemaConverter.ConvertDt(cinema));
        }
        public ResponseObject<DataResponsesCinema> UpdateCinema(Requests_UpdateCinema requests)
        {
            var updatecinema = _appDbContext.Cinemas.FirstOrDefault(x => x.Id == requests.cinemaId);
            if (requests.cinemaId == null) { return responsescinema.ResponseFail(StatusCodes.Status400BadRequest, "Không tìm thấy cinemaId !!", null); }
            updatecinema.Address = requests.Address;
            updatecinema.Description = requests.Description;
            updatecinema.Code = requests.Code;
            updatecinema.NameOfCinema = requests.NameOfCinema;
            if (updatecinema.Room != null) { _appDbContext.Rooms.RemoveRange(updatecinema.Room);}
            _appDbContext.Cinemas.Update(updatecinema);
            _appDbContext.SaveChanges();
            if (requests.Request_UpdateRooms != null) 
            {
                var rooms = _iRoomServices.CreateListRoom(updatecinema.Id, requests.Request_UpdateRooms);
                updatecinema.Room = rooms;
            }
            else{ updatecinema.Room = null; }
            _appDbContext.Cinemas.Update(updatecinema);
            _appDbContext.SaveChanges();
            return responsescinema.ResponseSucess("Cập Nhật Thông Tin Phim Thành Công!", cinemaConverter.ConvertDt(updatecinema));
        }
    }

}

