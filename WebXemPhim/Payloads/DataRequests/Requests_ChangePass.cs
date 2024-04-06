using System.ComponentModel.DataAnnotations;

namespace WebXemPhim.Payloads.DataRequests
{
    public class Requests_ChangePass
    {

        public string NewPassword { get; set; }
   
        public string ConfirmPassword { get; set; }
    }
}
