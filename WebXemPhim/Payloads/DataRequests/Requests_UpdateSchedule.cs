namespace WebXemPhim.Payloads.DataRequests
{
    public class Requests_UpdateSchedule
    {
        public int ScheduleId { get; set; }
        public int MovieId { get; set; }
        public DateTime StartAt { get; set; }
        public int RoomId { get; set; }
    }
}
