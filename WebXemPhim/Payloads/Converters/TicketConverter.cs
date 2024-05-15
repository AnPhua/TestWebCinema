﻿using Microsoft.EntityFrameworkCore;
using WebXemPhim.Entities;
using WebXemPhim.Payloads.DataResponses;

namespace WebXemPhim.Payloads.Converters
{
    public class TicketConverter
    {
        private readonly AppDbContext _context;
        public TicketConverter()
        {
            _context = new AppDbContext();
        }
        public DataResponsesTicket ConvertDt(Ticket ticket)
        {
            return new DataResponsesTicket
            {
                Code = ticket.Code,
                Id = ticket.Id,
                ScheduleName = _context.Schedules.SingleOrDefault(x => x.Id == ticket.ScheduleId).Name,
                SeatName = _context.Seats.SingleOrDefault(x => x.Id == ticket.SeatId).Line + _context.Seats.SingleOrDefault(x => x.Id == ticket.SeatId).Number,
                PriceTicket = ticket.PriceTicket
            };
        }
        public DataResponsesTicket ConvertDtandSeaType(Ticket ticket)
        {
            var seat = _context.Seats.SingleOrDefault(x => x.Id == ticket.SeatId);
            var seatType = seat != null ? _context.SeatTypes.SingleOrDefault(x => x.Id == seat.SeatTypeId) : null;
            if (seat != null && seatType != null)
            {
                return new DataResponsesTicket
                {
                    Code = ticket.Code,
                    Id = ticket.Id,
                    SeatType = seatType.NameType,
                    ScheduleName = _context.Schedules.SingleOrDefault(x => x.Id == ticket.ScheduleId)?.Name,
                    SeatName = seat.Line + seat.Number,
                    PriceTicket = ticket.PriceTicket
                };
            }
            return null;
        }
    }
}
