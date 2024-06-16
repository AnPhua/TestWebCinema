using WebXemPhim.Entities;

namespace WebXemPhim.Payloads.DataResponses
{
    public class DataResponsesMovieDetailsSchedule : DataResponsesId
    {
        public int MovieId { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string DayDetails { get; set; }
        public string Date { get; set; }
        public int MovieDuration { get; set; }
        public string Image { get; set; }
        public string MovieTypeName { get; set; }
        public string Name { get; set; }
        public string RateCode { get; set; }
        public string Trailer { get; set; }
        public IQueryable<ControlDatePlus> ListTimeinSchedules { get; set; }
    }
}
