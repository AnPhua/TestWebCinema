using Microsoft.EntityFrameworkCore;
using WebXemPhim.Entities;
using WebXemPhim.Payloads.DataResponses;

namespace WebXemPhim.Payloads.Converters
{
    public class CinemaConverter
    {
        private readonly AppDbContext _context;
        private readonly RoomConverter _roomConverter;
        public CinemaConverter()
        {
            _roomConverter = new RoomConverter();
            _context = new AppDbContext();
        }
        public DataResponsesCinema ConvertDt(Cinema cinema)
        {
            return new DataResponsesCinema
            {
                Id = cinema.Id,
                Address = cinema.Address,
                Description = cinema.Description,
                NameOfCinema = cinema.NameOfCinema,
                Room = _context.Rooms.Where(x => x.CinemaId == cinema.Id).Select(x => _roomConverter.ConvertDt(x)).AsQueryable(),
            };
        }
    }
}

