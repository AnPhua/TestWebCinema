using Azure.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Security.Claims;
using WebXemPhim.Entities;
using WebXemPhim.Payloads.DataRequests;
using WebXemPhim.Services.Interfaces;

namespace WebXemPhim.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }
        [HttpPost("register")]
        public IActionResult Register([FromBody]Requests_Register requests)
        {
            var result = _userServices.Register(requests);
            if(result.Status == 400) { return BadRequest(result); }
            else if (result.Status == 404) { return NotFound(result); }
            return  Ok(result);

        }
        [HttpPost("confirmNewAcc")]
        public IActionResult ConfirmNewAcc([FromBody]Requests_ConfirmEmail requests)
        {
            return Ok(_userServices.ConfirmNewAcc(requests));
        }
        [HttpPost("login")]
        public IActionResult LoginaAcc([FromBody]Requests_Login requests)
        {
            var result = _userServices.LoginAcc(requests);
            if (result.Status == 400) { return BadRequest(result); }
            else if (result.Status == 404) { return NotFound(result); }
            else if (result.Status == 500) { return NotFound(result); }
            return Ok(result);
        }
        [HttpPost("confirmemaillink")]
        public IActionResult ConfirmEmailusingLink([FromBody] Requests_RsPass requests)
        {
            var result = _userServices.ConfirmEmailLink(requests);
            return Ok(result);
        }
        [HttpGet("Authentication/reset-password/token/{token}/email/{email}/code/{code}")]
        public IActionResult GetLinkfromemail(string token, string email,string code)
        {
            var form = $@"<!DOCTYPE html>
                    <html>
                    <head>
                        <meta name=""viewport"" content=""width-device-width"" />
                        <title>ResetPassword</title>
                    </head>
                    <body>
                    <h1>Reset Password</h1>
                    <form action=""/api/User/resetpass/{code}"" method=""post"">
                        <input type=""hidden"" name=""Id"" value={code} />
                        <table>
                            <tr>
                                <td>New Password:</td>
                                <td>
                                    <input type=""password"" name=""NewPassword"" />
                                </td>
                            </tr>
                            <tr>
                                <td>Confirm Password: </td>
                                <td>
                                    <input type=""password"" name=""ConfirmPassword"" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <button type=""submit"">Submit</button>
                                </td>
                            </tr>
                        </table>
                    </form>
                </body>
                </html>";

            return Content(form, "text/html");
        }
        [HttpPost("resetpass/{code}")]
        public IActionResult ResetPass(string code)
        {
            var newPassword = Request.Form["NewPassword"];
            var confirmPassword = Request.Form["ConfirmPassword"];
            var requests = new Requests_ChangePass
            {
                NewPassword = newPassword,
                ConfirmPassword = confirmPassword
            };
            var result = _userServices.ResetPasswordconfirmlink(code, requests);
            return Ok(result);
        }
        [HttpGet("getallinformation")]
        [Authorize(Roles = "Admin,Censor")]
        public IActionResult GetAllInformation()
        {
            var results = _userServices.GetAllInfomation(); 
            return Ok(results); 
        }
        //[HttpPut("changepassword")] // check đăng nhập
        //[Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        //public IActionResult ChangePassword([FromForm]Requests_ChangePass requests)
        //{
        //    int id = int.Parse(HttpContext.User.FindFirst("UserId").Value);
        //    return Ok(_userServices.ChangeYourPassword(id, requests));
        //}

    }
}
