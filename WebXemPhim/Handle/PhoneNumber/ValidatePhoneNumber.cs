﻿using System.Text.RegularExpressions;

namespace WebXemPhim.Handle.PhoneNumber
{
    public class ValidatePhoneNumber
    {
        public static bool IsValidPN(string pn)
        {
            string checkPhoneNumber = @"^[0-9]*$";
            return Regex.IsMatch(pn, checkPhoneNumber);
        }
    }
}
