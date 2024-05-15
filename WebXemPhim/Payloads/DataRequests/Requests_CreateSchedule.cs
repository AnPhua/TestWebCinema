namespace WebXemPhim.Payloads.DataRequests
{
    public class Requests_CreateSchedule
    {
        public int MovieId { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public int RoomId { get; set; }
    }
}
