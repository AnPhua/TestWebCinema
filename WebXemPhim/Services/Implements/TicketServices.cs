using Microsoft.EntityFrameworkCore;
using WebXemPhim.Entities;
using WebXemPhim.Handle.HandlePagination;
using WebXemPhim.Payloads.Converters;
using WebXemPhim.Payloads.DataRequests;
using WebXemPhim.Payloads.DataResponses;
using WebXemPhim.Payloads.Responses;
using WebXemPhim.Services.Implements;
using WebXemPhim.Services.Interfaces;

namespace MovieManagement.Services.Implements
{
    public class TicketServices :BaseServices, ITicketServices
    {
        private readonly ResponseObject<DataResponsesTicket> _responseObject;
        private readonly TicketConverter _ticketConverter;
        private readonly SchedulesConverter _scheduleConverter;
        public TicketServices(ResponseObject<DataResponsesTicket> responseObject, 
            TicketConverter ticketConverter,
            SchedulesConverter scheduleConverter)
        {
            _responseObject = responseObject;
            _ticketConverter = ticketConverter;
            _scheduleConverter = scheduleConverter;
        }

        public async Task<ResponseObject<DataResponsesTicket>> CreateTicket(int scheduleId, Requests_CreateTicket request)
        {
            var seat = await _appDbContext.Seats.SingleOrDefaultAsync(x => x.Id == request.SeatId);
            if (seat == null)
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Không Tìm Thấy Ghế", null);
            }
            var schedule = await _appDbContext.Schedules.SingleOrDefaultAsync(x => x.Id == scheduleId);
            if(schedule == null)
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Không Tìm Thấy Lịch Chiếu", null);
            }
            if(_appDbContext.Tickets.Any(x=>x.SeatId == request.SeatId && x.ScheduleId == scheduleId))
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Không Thể Chọn Trùng Ghế!", null);
            }
            if (seat.RoomId != schedule.RoomId)
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Hai Phòng Chiếu Không Khớp", null);
            }
            Ticket ticket = new Ticket();
            ticket.ScheduleId = scheduleId;
            ticket.TypeTicket = 1;
            ticket.SeatId = request.SeatId;
            ticket.Code = "Movie" + DateTime.Now.Ticks.ToString() + new Random().Next(1000, 9999).ToString();
            if (seat.SeatTypeId == 1)
            {
                ticket.PriceTicket = 45000;
            }
            else if (seat.SeatTypeId == 2)
            {
                ticket.PriceTicket = 50000;
            }
            else if (seat.SeatTypeId == 3)
            {
                ticket.PriceTicket = 120000;
            }
            else
            { 
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Loại Ghế Không Hợp Lệ", null);
            }
            await _appDbContext.Tickets.AddAsync(ticket);
            await _appDbContext.SaveChangesAsync();
            return _responseObject.ResponseSucess("Tạo Vé Thành Công", _ticketConverter.ConvertDt(ticket));
        }

        public async Task<ResponseObject<DataResponsesTicket>> UpdateTicket(Requests_UpdateTicket request)
        {
            var ticket = await _appDbContext.Tickets.SingleOrDefaultAsync(x => x.Id == request.TicketId);
            if(ticket == null)
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Không tìm thấy thông tin", null);
            }
            ticket.Id = request.TicketId;
            ticket.ScheduleId = request.ScheduleId;
            ticket.SeatId= request.SeatId;
            _appDbContext.Tickets.Update(ticket);
            await _appDbContext.SaveChangesAsync();
            return _responseObject.ResponseSucess("Cập nhật thông tin thành công", _ticketConverter.ConvertDt(ticket));
        }

        //public async Task<List<Ticket>> CreateListTicket(int scheduleId, List<Requests_CreateTicket> requests)
        //{
        //    var schedule = await _appDbContext.Schedules.SingleOrDefaultAsync(x => x.Id == scheduleId);
        //    if (schedule == null)
        //    {
        //        throw new ArgumentNullException("Lịch chiếu không tồn tại");
        //    }

        //    List<Ticket> list = new List<Ticket>();

        //    foreach (var request in requests)
        //    {
        //        var seat = await _appDbContext.Seats.SingleOrDefaultAsync(x => x.Id == request.SeatId);
        //        if (seat == null)
        //        {
        //            throw new ArgumentException("Không Tìm Thấy Ghế");
        //        }

        //        if (_appDbContext.Tickets.Any(x => x.SeatId == request.SeatId && x.ScheduleId == scheduleId))
        //        {
        //            throw new InvalidOperationException("Không Thể Chọn Trùng Ghế!");
        //        }

        //        if (seat.RoomId != schedule.RoomId)
        //        {
        //            throw new InvalidOperationException("Hai Phòng Chiếu Không Khớp");
        //        }

        //        Ticket ticket = new Ticket
        //        {
        //            ScheduleId = scheduleId,
        //            SeatId = request.SeatId,
        //            Code = "Movie" + DateTime.Now.Ticks.ToString() + new Random().Next(1000, 9999).ToString()
        //        };

        //        switch (seat.SeatTypeId)
        //        {
        //            case 1:
        //                ticket.PriceTicket = 45000;
        //                break;
        //            case 2:
        //                ticket.PriceTicket = 50000;
        //                break;
        //            case 3:
        //                ticket.PriceTicket = 120000;
        //                break;
        //            default:
        //                throw new ArgumentException("Loại Ghế Không Hợp Lệ");
        //        }

        //        list.Add(ticket);
        //    }

        //    await _appDbContext.Tickets.AddRangeAsync(list);
        //    await _appDbContext.SaveChangesAsync();
        //    return list;
        //}

        public async Task<List<Ticket>> CreateListTicket(int scheduleId)
        {
            var schedule = await _appDbContext.Schedules.SingleOrDefaultAsync(x => x.Id == scheduleId);
            if (schedule == null)
            {
                throw new ArgumentNullException("Lịch chiếu không tồn tại");
            }

            var seatsInRoom = await _appDbContext.Seats
                .Where(x => x.RoomId == schedule.RoomId)
                .ToListAsync();

            var existingTickets = await _appDbContext.Tickets
                .Where(x => x.ScheduleId == scheduleId)
                .Select(x => x.SeatId)
                .ToListAsync();

            List<Ticket> list = new List<Ticket>();

            foreach (var seat in seatsInRoom)
            {
                if (existingTickets.Contains(seat.Id))
                {
                    continue; 
                }

                Ticket ticket = new Ticket
                {
                    ScheduleId = scheduleId,
                    SeatId = seat.Id,
                    TypeTicket = 1,
                    Code = "Movie" + DateTime.Now.Ticks.ToString() + new Random().Next(1000, 9999).ToString()
                };

                switch (seat.SeatTypeId)
                {
                    case 1:
                        ticket.PriceTicket = 45000;
                        break;
                    case 2:
                        ticket.PriceTicket = 50000;
                        break;
                    case 3:
                        ticket.PriceTicket = 120000;
                        break;
                    default:
                        throw new ArgumentException("Loại Ghế Không Hợp Lệ");
                }

                list.Add(ticket);
            }

            await _appDbContext.Tickets.AddRangeAsync(list);
            await _appDbContext.SaveChangesAsync();
            return list;
        }


        public async Task<DataResponsesTicket[]> GetAllTicketNoPagination(int scheduleId)
        {
            var tickets = await _appDbContext.Tickets
                    .Where(x => x.ScheduleId == scheduleId)
                    .Select(x => _ticketConverter.ConvertDtandSeaType(x))
                    .ToArrayAsync();
            return tickets;
        }
        public async Task<PageResult<DataResponsesSchedule>> GetAllTicketss(int pageSize, int pageNumber)
        {
            var query = _appDbContext.Schedules.Where(x=>x.IsActive == true).Select(x => _scheduleConverter.ConvertDtforticket(x));
            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;
        }

    }
}
