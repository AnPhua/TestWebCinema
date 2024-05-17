using Microsoft.EntityFrameworkCore;
using WebXemPhim.Entities;
using WebXemPhim.Handle.HandlePagination;
using WebXemPhim.Payloads.Converters;
using WebXemPhim.Payloads.DataRequests;
using WebXemPhim.Payloads.DataResponses;
using WebXemPhim.Payloads.Responses;
using WebXemPhim.Services.Interfaces;

namespace WebXemPhim.Services.Implements
{
    public class SeatServices:BaseServices,ISeatServices
    {
        private  readonly ResponseObject<DataResponsesRoom> responseObject;
        private readonly SeatConverter _seatConverter;
        private readonly RoomConverter _roomConverter;
        private readonly ResponseObject<DataResponsesSeat> _responseObject;
        public SeatServices(ResponseObject<DataResponsesRoom> responseObject,SeatConverter _seatConverter, RoomConverter _roomConverter, ResponseObject<DataResponsesSeat> _responseObject) { 
            this.responseObject = responseObject;
            this._seatConverter = _seatConverter;
            this._roomConverter = _roomConverter;
            this._responseObject = _responseObject;
        }
        //public async Task<List<Seat>> CreateListSeat(int roomId, List<Requests_CreateSeat> requests)
        //{
        //    var room = await _appDbContext.Rooms.SingleOrDefaultAsync(x => x.Id == roomId);
        //    if (room == null)
        //    {
        //        throw new ArgumentNullException("Lịch chiếu không tồn tại");
        //    }

        //    List<Seat> listseat = new List<Seat>();

        //    var groupedRequests = requests.GroupBy(r => r.Line);

        //    foreach (var group in groupedRequests)
        //    {
        //        var line = group.Key;
        //        var seatNumbers = await _appDbContext.Seats
        //                                             .Where(x => x.RoomId == roomId && x.Line == line)
        //                                             .Select(x => x.Number)
        //                                             .ToListAsync();

        //        var maxNumber = seatNumbers.DefaultIfEmpty(0).Max();  

        //        foreach (var request in group)
        //        {
        //            if (maxNumber > 20)
        //            {
        //                throw new InvalidOperationException($"Tối Đa Mỗi Hàng Chỉ 20 Ghế tại hàng {line}");
        //            }

        //            Seat seat = new Seat()
        //            {
        //                Line = line,
        //                Number = ++maxNumber,
        //                SeatStatusId = 1,
        //                SeatTypeId = request.SeatTypeId,
        //                RoomId = roomId,
        //                IsActive = true
        //            };

        //            listseat.Add(seat);
        //        }
        //    }

        //    await _appDbContext.Seats.AddRangeAsync(listseat);
        //    await _appDbContext.SaveChangesAsync();
        //    return listseat;
        //}
        public async Task<List<Seat>> CreateListSeat(int roomId, List<Requests_CreateSeat> requests)
        {
            var room = await _appDbContext.Rooms.SingleOrDefaultAsync(x => x.Id == roomId);
            if (room == null)
            {
                throw new ArgumentNullException("Phòng không tồn tại");
            }

            List<Seat> listseat = new List<Seat>();

            var groupedRequests = requests.GroupBy(r => r.Line);

            foreach (var group in groupedRequests)
            {
                var line = group.Key;
                var existingSeats = await _appDbContext.Seats
                                                       .Where(x => x.RoomId == roomId && x.Line == line)
                                                       .ToListAsync();

                var maxNumber = existingSeats.Select(x => x.Number).DefaultIfEmpty(0).Max();
                var doubleSeatCount = existingSeats.Count(x => x.SeatTypeId == 3);
                bool hasVipOrRegular = existingSeats.Any(x => x.SeatTypeId == 1 || x.SeatTypeId == 2);
                foreach (var request in group)
                {
                    if (request.SeatTypeId == 3 && doubleSeatCount >= 4)
                    {
                        throw new InvalidOperationException($"Tối Đa Mỗi Hàng Chỉ 4 Đối Với Ghế Đôi Ở Hàng {line}");
                    }

                    if (maxNumber >= 14)
                    {
                        throw new InvalidOperationException($"Tối Đa Mỗi Hàng Chỉ 14 Ghế Ở Hàng {line}");
                    }
                    if (request.SeatTypeId == 3 && hasVipOrRegular)
                    {
                        throw new InvalidOperationException($"Không Thể Thêm Ghế Đôi Vào Hàng {line} Vì Đã Có Ghế VIP Hoặc Ghế Thường.");
                    }

                    if ((request.SeatTypeId == 1 || request.SeatTypeId == 2) && doubleSeatCount > 0)
                    {
                        throw new InvalidOperationException($"Không Thể Thêm Ghế VIP Hoặc Ghế Thường Vào Hàng {line} Vì Đã Có Ghế Đôi.");
                    }

                    Seat seat = new Seat()
                    {
                        Line = line,
                        Number = ++maxNumber,
                        SeatStatusId = 1,
                        SeatTypeId = request.SeatTypeId,
                        RoomId = roomId,
                        IsActive = true
                    };

                    listseat.Add(seat);

                    if (seat.SeatTypeId == 3)
                    {
                        doubleSeatCount++;
                    }
                }
            }

            await _appDbContext.Seats.AddRangeAsync(listseat);
            await _appDbContext.SaveChangesAsync();
            return listseat;
        }


        public ResponseObject<DataResponsesSeat> CreateSeat(int roomId, Requests_CreateSeat request)
        {
            var room = _appDbContext.Rooms.SingleOrDefault(x => x.Id == roomId);
            if (room == null)
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Không Tìm Thấy Phòng", null);
            }

            var seatNumbers = _appDbContext.Seats.Where(x => x.RoomId == roomId && x.Line == request.Line)
                                                 .Select(x => x.Number)
                                                 .ToList();

            var maxNumber = seatNumbers.DefaultIfEmpty(1).Max();

            if (maxNumber >= 20)
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Tối Đa Mỗi Hàng Chỉ 20 Ghế", null);
            }

            Seat seat = new Seat()
            {
                Line = request.Line,
                Number = maxNumber + 1,
                SeatStatusId = 1,
                SeatTypeId = request.SeatTypeId,
                RoomId = roomId
                
            };

            _appDbContext.Seats.Add(seat);
            _appDbContext.SaveChanges();
            return _responseObject.ResponseSucess("Thêm Ghế Thành Công", _seatConverter.ConvertDt(seat));
        }


        public ResponseObject<DataResponsesRoom> UpdateSeat(int roomId, List<Requests_UpdateSeats> requests)
        {
            var room =  _appDbContext.Rooms.Include(x => x.Seats).SingleOrDefault(x => x.Id == roomId);
            if (room == null)
            {
                return responseObject.ResponseFail(StatusCodes.Status404NotFound, "Không tìm thấy phòng", null);
            }

            var seatDict = room.Seats.ToDictionary(s => s.Id, s => s);

            foreach (var request in requests)
            {
                if (!seatDict.TryGetValue(request.SeatId, out var seat))
                {
                    return responseObject.ResponseFail(StatusCodes.Status404NotFound, "Không tìm thấy ghế", null);
                }
                seat.SeatStatusId = request.SeatStatusId;
                seat.RoomId = roomId;
                seat.Number = request.Number;
                seat.Line = request.Line;
                seat.SeatTypeId = request.SeatTypeId;
                _appDbContext.Seats.Update(seat);
            }

             _appDbContext.SaveChanges();
            return responseObject.ResponseSucess("Cập Nhật Thông Tin Ghế Trong Phòng Thành Công", _roomConverter.ConvertDt(room));
        }

        public string DeleteSeat(int seatId)
        {
            var seatdelete = _appDbContext.Seats.SingleOrDefault(s => s.Id == seatId);
            if (seatdelete is null) { return "Không Tìm Thấy Id Ghế!"; }
            seatdelete.IsActive = false;
            _appDbContext.Seats.Update(seatdelete);
            _appDbContext.SaveChanges();
            return "Xóa Ghế Thành Công!";
        }
        public async Task<PageResult<DataResponsesRoom>> GetAllSeat(int pageSize, int pageNumber)
        {
            var query = _appDbContext.Rooms.Where(x => x.IsActive == true).Select(x => _roomConverter.ConvertDt(x));
            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;
        }

        public IEnumerable<SeatType> GetSeatTypes()
        {
            return _appDbContext.SeatTypes.AsQueryable();
        }
    }
}
