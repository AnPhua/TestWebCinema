﻿namespace WebXemPhim.Entities
{
    public class Ticket : BaseId
    {
        public string Code { get; set; }
        public int ScheduleId { get; set; }
        public int SeatId { get; set; }
        public int TypeTicket { get; set; }
        public double PriceTicket { get; set; }
        public bool? IsActive { get; set; } = true;
        public Schedule? Schedule { get; set; }
        public Seat? Seat { get; set; }
        public IEnumerable<BillTicket>? BillTickets { get; set; }
    }
}
