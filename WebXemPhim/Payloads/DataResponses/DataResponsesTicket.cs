using WebXemPhim.Entities;

namespace WebXemPhim.Payloads.DataResponses
{
    public class DataResponsesTicket : DataResponsesId
    {
        public string Code { get; set; }
        public string ScheduleName { get; set; }
        public string SeatName { get; set; }
        public string SeatType { get; set; }
        public double PriceTicket { get; set; }
        public IQueryable<BillTicket> BillTickets { get; set; }
    }
}
