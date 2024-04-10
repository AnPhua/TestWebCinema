using Microsoft.EntityFrameworkCore;
using WebXemPhim.Entities;
using WebXemPhim.Payloads.DataRequests;
using WebXemPhim.Payloads.DataResponses;
using WebXemPhim.Payloads.Responses;
using WebXemPhim.Services.Implements;
using WebXemPhim.Services.Interfaces;

namespace WebXemPhim.Payloads.Converters
{
    public class SeatServices : BaseServices, ISeatServices
    {
        private readonly ResponseObject<DataResponsesSeat> _responseseat;
        private readonly ResponseObject<DataResponsesRoom> _responseObjectRoom;
        private readonly SeatConverter _seatConverter;
        private readonly RoomConverter _roomConverter;
        public SeatServices(ResponseObject<DataResponsesSeat> responseseat, ResponseObject<DataResponsesRoom> responseObjectRoom, SeatConverter _seatConverter, RoomConverter roomConverter)
        {
            _responseseat = responseseat;
            _responseObjectRoom = responseObjectRoom;
            this._seatConverter = _seatConverter;
            _roomConverter = roomConverter;
        }
        public List<Seat> CreateListSeat(int roomId, List<Requests_CreateSeat> requests)
        {
            var room = _appDbContext.Rooms.SingleOrDefault(x => x.Id == roomId);
            if (room is null)
            {
                return null;
            }
            List<Seat> listseat = new List<Seat>();
            foreach (var request in requests)
            {
                Seat seat = new Seat();
                seat.SeatStatusId = 1;// seats are empty
                seat.Line = request.Line;
                seat.Number = request.Number;
                seat.RoomId = roomId;
                seat.SeatTypeId = request.SeatTypeId;
                seat.IsActive = true;
                listseat.Add(seat);
            }
            _appDbContext.Seats.AddRange(listseat);
            _appDbContext.SaveChanges();
            return listseat;
        }

        public ResponseObject<DataResponsesSeat> CreateSeat(int roomId, Requests_CreateSeat request)
        {
            var room = _appDbContext.Rooms.SingleOrDefault(x => x.Id == roomId);
            if (room is null)
            {
                return _responseseat.ResponseFail(StatusCodes.Status404NotFound, "Không tìm thấy id của phòng", null);
            }
            Seat seat = new Seat()
            {
                Line = request.Line,
                Number = request.Number,
                SeatTypeId = request.SeatTypeId,
                RoomId = roomId,
                SeatStatusId = 1,// seats are empty
            };
            _appDbContext.Seats.Add(seat);
            _appDbContext.SaveChanges();
            return _responseseat.ResponseSucess("Thêm ghế thành công", _seatConverter.ConvertDt(seat));
        }

        public ResponseObject<DataResponsesRoom> UpdateSeat(int roomId, List<Requests_UpdateSeats> requests)
        {
            var room =  _appDbContext.Rooms.Include(x => x.Seats).SingleOrDefault(x => x.Id == roomId);
            if (room == null)
            {
                return _responseObjectRoom.ResponseFail(StatusCodes.Status404NotFound, "Không tìm thấy phòng", null);
            }

            var seatDict = room.Seats.ToDictionary(s => s.Id, s => s);

            foreach (var request in requests)
            {
                if (!seatDict.TryGetValue(request.SeatId, out var seat))
                {
                    return _responseObjectRoom.ResponseFail(StatusCodes.Status404NotFound, "Không tìm thấy ghế", null);
                }
                seat.SeatStatusId = request.SeatStatusId;
                seat.RoomId = roomId;
                seat.Number = request.Number;
                seat.Line = request.Line;
                seat.SeatTypeId = request.SeatTypeId;

                _appDbContext.Seats.Update(seat);
            }

             _appDbContext.SaveChanges();
            return _responseObjectRoom.ResponseSucess("Cập nhật thông tin ghế trong phòng thành công", _roomConverter.ConvertDt(room));
        }
    }
}
