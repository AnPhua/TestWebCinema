namespace WebXemPhim.Payloads.DataResponses
{
    public class DataResponsesGetDays : DataResponsesId
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string DayDetails { get; set; }
        public string Date { get; set; }
    }
}
