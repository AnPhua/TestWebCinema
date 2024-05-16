using Microsoft.EntityFrameworkCore;
using WebXemPhim.Entities;
using WebXemPhim.Payloads.DataResponses;

namespace WebXemPhim.Payloads.Converters
{
    public class RoomConverter
    {
        private readonly SeatConverter _seatConverter;
        private readonly SchedulesConverter _scheduleConverter;
        private readonly AppDbContext _appDbContext;
        
        public RoomConverter()
        {
            _appDbContext = new AppDbContext();
            _seatConverter = new SeatConverter();
            _scheduleConverter = new SchedulesConverter();
        }
        public DataResponsesRoom ConvertDt(Room room)
        {
            return new DataResponsesRoom
            {
                Id = room.Id,
                Capacity = room.Capacity,
                Description = room.Description,
                Name = room.Name,
                Type = room.Type,
                CinemaName = _appDbContext.Cinemas.SingleOrDefault(x => x.Id == room.CinemaId).NameOfCinema,
                Code = room.Code,
                DataResponseSeats = _appDbContext.Seats.Where(x => x.RoomId == room.Id).Select(x => _seatConverter.ConvertDt(x)).AsQueryable(),
                DataResponseSchedules = _appDbContext.Schedules.Where(x => x.RoomId == room.Id).Select(x => _scheduleConverter.ConvertDt(x)).AsQueryable()
            };
        }
        public DataResponsesRoom ConvertDtforSeat(Room room)
        {
            return new DataResponsesRoom
            {
                Id = room.Id,
                Capacity = room.Capacity,
                Description = room.Description,
                Name = room.Name,
                CinemaName = _appDbContext.Cinemas.SingleOrDefault(x => x.Id == room.CinemaId).NameOfCinema,
                DataResponseSeats = _appDbContext.Seats.Where(x => x.RoomId == room.Id).OrderBy(x=>x.Line).Select(x => _seatConverter.ConvertDt(x)).AsQueryable(),
                DataResponseSchedules = _appDbContext.Schedules.Where(x => x.RoomId == room.Id).Select(x => _scheduleConverter.ConvertDt(x)).AsQueryable()
            };
        }
    }
}
