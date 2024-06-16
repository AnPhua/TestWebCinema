namespace WebXemPhim.Entities
{
    public class ControlDatePlus : BaseId
    {
        public string TimeDt { get; set; }
        public string RoomName { get; set; }
        public DateTime StartAt { get; set; }
        public int EmptySeat { get; set; }
    }
}
