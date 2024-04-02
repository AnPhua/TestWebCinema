using System.ComponentModel.DataAnnotations;

namespace WebXemPhim.Handle.Email
{
    public class ValidateEmail
    {
        public static bool IsValidEmail(string email)
        {
            var checkEmail = new EmailAddressAttribute();
            return checkEmail.IsValid(email);
        }
    }
}
