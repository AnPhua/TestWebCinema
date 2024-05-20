using WebXemPhim.Entities;

namespace WebXemPhim.Payloads.DataResponses
{
    public class DataResponsesTicket : DataResponsesId
    {
        //public string Code { get; set; }
        public string ScheduleName { get; set; }
        public string SeatName { get; set; }
        //public string SeatType { get; set; }
        public int TypeTicket { get; set; }
        public int SeatId { get; set; }
        public string Line { get; set; }
        public bool? IsActive { get; set; }
        public int SeatTypeId { get; set; }
        public double PriceTicket { get; set; }
        //public IQueryable<BillTicket> BillTickets { get; set; }
    }
}
