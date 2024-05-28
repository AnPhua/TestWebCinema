namespace WebXemPhim.Entities
{
    public class ControlDate :BaseId
    {
        public string TimeDt { get; set; }
        public DateTime StartAt { get; set; }
        public int EmptySeat { get; set; }
    }
}
