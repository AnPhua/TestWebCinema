using System.ComponentModel.DataAnnotations;

namespace WebXemPhim.Payloads.DataRequests
{
    public class Requests_UpdateFoodhavestring
    {
        public int FoodId { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string NameOfFood { get; set; }
    }
}
