using WebXemPhim.Payloads.DataResponses;

namespace WebXemPhim.Payloads.DataResponses
{
    public class DataResponsesBillFood : DataResponsesId
    {
        public int Quantity { get; set; }
        public string NameOfFood { get; set; }
    }
}
