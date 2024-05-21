namespace WebXemPhim.Payloads.DataRequests
{
    public class Requests_CreateBill
    {
        public int CustomerId { get; set; }
        public int? PromotionId { get; set; }
        public List<Requests_CreateBillFood>? BillFoods { get; set; }
        public List<Requests_CreateBillTicket> BillTickets { get; set; }
    }
}
