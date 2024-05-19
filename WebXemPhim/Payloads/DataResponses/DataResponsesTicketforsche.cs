namespace WebXemPhim.Payloads.DataResponses
{
    public class DataResponsesTicketforsche : DataResponsesId
    {
        public string SeatName { get; set; }
        

        public int SeatId { get; set; }
        public int SeatTypeId { get; set; }
        public double PriceTicket { get; set; }
        public string Line { get; set; }
        public bool? IsActive { get; set; }
    }
}
