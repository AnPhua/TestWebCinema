using WebXemPhim.Payloads.DataRequests;

namespace WebXemPhim.Payloads.DataResponses
{
    public class DataResponsesSchedule : DataResponsesId
    {
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string Code { get; set; }
        public string MovieName { get; set; }
        public string Name { get; set; }
        public string RoomName { get; set; }
        public int EmptySeat { get; set; }
        public IQueryable<DataResponsesTicket> DataResponseTickets { get; set; }
        public IQueryable<DataResponsesTicketforsche> DataResponsesTicketforsche { get; set; }
    }
}
