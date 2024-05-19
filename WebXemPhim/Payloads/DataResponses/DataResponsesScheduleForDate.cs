using WebXemPhim.Entities;

namespace WebXemPhim.Payloads.DataResponses
{
    public class DataResponsesScheduleForDate : DataResponsesId
    {
        public int MovieId { get; set; }
        public string RoomName { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }   
        public string DayDetails { get; set; }
        public string Date { get; set; }
        public IQueryable<ControlDate> ListTimeinSchedules { get; set; }
    }
}
