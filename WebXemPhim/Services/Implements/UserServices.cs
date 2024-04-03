using WebXemPhim.Entities;
using WebXemPhim.Handle.Email;
using WebXemPhim.Handle.UserName;
using WebXemPhim.Handle.PhoneNumber;
using WebXemPhim.Handle.Name;
using WebXemPhim.Payloads.Converters;
using WebXemPhim.Payloads.DataRequests;
using WebXemPhim.Payloads.DataResponses;
using WebXemPhim.Payloads.Responses;
using WebXemPhim.Services.Interfaces;
using BCryptpw = BCrypt.Net.BCrypt;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Net;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Forms;
using System.Security.Cryptography;
using System.Net.WebSockets;
using Microsoft.Identity.Client;

namespace WebXemPhim.Services.Implements
{
    public class UserServices : BaseServices , IUserServices
    {
        
        private readonly ResponseObject<DataResponsesUser> _responseObject;
        private readonly UserConverter _converter;
        private readonly IConfiguration _configuration;
        private readonly ResponseObject<DataResponsesToken> _responseTokenObject;
        public UserServices(IConfiguration configuration)
        {
            _converter = new UserConverter();
            _responseObject = new ResponseObject<DataResponsesUser>();
            _configuration = configuration;
            _responseTokenObject = new ResponseObject<DataResponsesToken>();
        }
        private string RandomActiveCode()
        {
            Random rd = new Random();
            string character = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string code = new string(Enumerable.Repeat(character, 6).Select(s => s[rd.Next(s.Length)]).ToArray());
            return code;
        }
        public string SendMail(SendEmail e)
        {
            var mail = "anphu0220@gmail.com";
            var password = "ltevkzyodqwisktm";

            if (!ValidateEmail.IsValidEmail(e.Email))
            {
                return "Định Dạng Email Không Đúng";
            }

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port=587,
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password),
            };
            try
            {
                smtpClient.Send(new MailMessage(
                    from: mail,
                    to: e.Email,
                    subject: e.Title,
                    body: e.Body)
                {
                    IsBodyHtml = true
                });

                return "Gửi Mã Về Mail Thành Công, Nhập Mã Để Xác Thực!";
            }
            catch (Exception ex)
            {
                return "Lỗi Email: " + ex.Message;
            }
        }
        public ResponseObject<DataResponsesUser> Register(Requests_Register requests)
        {
            if (string.IsNullOrEmpty(requests.Username) ||
                string.IsNullOrEmpty(requests.Email) ||
                string.IsNullOrEmpty(requests.Name) ||
                string.IsNullOrEmpty(requests.PhoneNumber) ||
                string.IsNullOrEmpty(requests.Password))
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Thông Tin Cần Nhập Đầy Đủ", null);
            }
            if(_appDbContext.Users.Any(x => x.Email.Equals(requests.Email)))
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Email Này Đã Tồn Tại", null);
            }
            if (_appDbContext.Users.Any(x => x.Username.Equals(requests.Username)))
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Username Này Đã Tồn Tại", null);
            }
            if (!ValidateEmail.IsValidEmail(requests.Email))
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Định Dạng Email Không Đúng", null);
            }
            if (!ValidateUserName.IsValidUser(requests.Username))
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Định Dạng UserName Không Đúng", null);
            }
            if (!ValidatePhoneNumber.IsValidPN(requests.PhoneNumber))
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Định Dạng PhoneNumber Không Đúng", null);
            }
            if (!ValidateName.IsValidName(requests.Name))
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Định Dạng Name không đúng", null);
            }
            var user = new User();
            user.Email = requests.Email;
            user.Name = requests.Name;
            user.PhoneNumber = requests.PhoneNumber;
            user.Password = BCryptpw.HashPassword(requests.Password);
            user.Username = requests.Username;
            user.IsActive = false;
            user.RoleId = 3;
            user.UserStatusId = 1;
            user.Point = 0;
            _appDbContext.Users.Add(user);
            _appDbContext.SaveChanges();
            ConfirmEmail confirmEmail = new ConfirmEmail();
            confirmEmail.UserId = user.Id;
            confirmEmail.ExpiredTime = DateTime.UtcNow.AddMinutes(30);
            confirmEmail.ConfirmCode = RandomActiveCode();
            confirmEmail.IsConfirm = false;
            _appDbContext.ConfirmEmails.Add(confirmEmail);
            _appDbContext.SaveChanges();
            DataResponsesUser result = _converter.ConvertDt(user);
            string message = SendMail(new SendEmail
            {
                Email = requests.Email,
                Title = "KÍCH HOẠT TÀI KHOẢN MỚI: ",
                Body = "MÃ KÍCH HOẠT:[<strong style='font-weight:900; font-size:25px;'>" + confirmEmail.ConfirmCode + "</strong>] Có hiệu lực 30 phút"
            });
            return _responseObject.ResponseSucess("Đăng Ký Tài Khoản Thành Công!Hãy Kiểm Tra Mail Để Xác Thực Tài Khoản" , result);    

        }
        public ResponseObject<DataResponsesUser> ConfirmNewAcc(Requests_ConfirmEmail requests)
        {
            ConfirmEmail confirmEmail = _appDbContext.ConfirmEmails.Where(x => x.ConfirmCode.Equals(requests.ConfirmCode)).SingleOrDefault();
            if (confirmEmail == null)
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Mã Xác Thực Không Đúng ,Xác Minh Tài Khoản Thất Bại", null);
            }
            if (confirmEmail.ExpiredTime < DateTime.UtcNow)
            {
                User userdetele = _appDbContext.Users.FirstOrDefault(x => x.Id == confirmEmail.UserId);
                _appDbContext.ConfirmEmails.Remove(confirmEmail);
                _appDbContext.Users.Remove(userdetele);
                _appDbContext.SaveChanges();
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Mã Xác Thực Hết Hiệu Lực ! Hãy Đăng Kí Lại Tài Khoản (T.T)", null);
            }
            User user = _appDbContext.Users.FirstOrDefault(x => x.Id == confirmEmail.UserId);
            user.UserStatusId = 2;
            user.IsActive = true;
            _appDbContext.ConfirmEmails.Remove(confirmEmail);
            _appDbContext.Users.Update(user);
            _appDbContext.SaveChangesAsync();
            DataResponsesUser result = _converter.ConvertDt(user);
            return _responseObject.ResponseSucess("Xác Thực Tài Khoản Thành Công!", result);
        }

        private string GenerateRefreshToken()
        {
            var random = new byte[64];
            using(var item = RandomNumberGenerator.Create())
            {
                item.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }
        public DataResponsesToken GenerateAccessToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyByte = System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:SecretKey").Value);

            var role = _appDbContext.Roles.SingleOrDefault(x => x.Id == user.RoleId);

            var tokenDescription = new SecurityTokenDescriptor // mô tả về token
            {
                Subject = new System.Security.Claims.ClaimsIdentity(
                new[]
                {
                    new Claim("Id",user.Id.ToString()),
                    new Claim("Email",user.Email),
                    new Claim("Role-Name",role.Code),
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyByte),SecurityAlgorithms.HmacSha256Signature)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescription);//Tạo ra token dựa trên token đã mô tả
            var accessToken = jwtTokenHandler.WriteToken(token);//Security token chuyeen sang string
            var refreshToKen = GenerateRefreshToken();
            RefreshToken rf = new RefreshToken
            {
                Token = refreshToKen,
                ExpiredTime = DateTime.UtcNow.AddHours(2),
                UserId = user.Id,
            };//lamf moi accesstoken khi no het han
            _appDbContext.RefreshTokens.Add(rf);
            _appDbContext.SaveChanges();
            DataResponsesToken result = new DataResponsesToken
            {
                AccessToken = accessToken,
                RefreshToken = refreshToKen,
            };
            return result;
        }

        public DataResponsesToken RestartAccessToKen(Requests_RestartToken requests)
        {
            throw new NotImplementedException();
        }

        public ResponseObject<DataResponsesToken> LoginAcc(Requests_Login requests)
        {
            var user = _appDbContext.Users.SingleOrDefault(x => x.Username.Equals(requests.UserName));
            bool checkPass = BCryptpw.Verify(requests.Password, user.Password);
            if (string.IsNullOrEmpty(requests.UserName)||
               string.IsNullOrEmpty(requests.Password))
            {
                return _responseTokenObject.ResponseFail(StatusCodes.Status400BadRequest, "Username và Password đang bị trống!",null);
            }
            if (user == null)
            {
                return _responseTokenObject.ResponseFail(StatusCodes.Status400BadRequest, "Username không tồn tại! T.T", null);
            }
            if (user.UserStatusId == 1) //có thể vào
            {
                return _responseTokenObject.ResponseFail(StatusCodes.Status401Unauthorized, "Tài Khoản của bạn chưa được kích hoạt!", null);
            }
            if (user.IsActive == false)
            {
                return _responseTokenObject.ResponseFail(StatusCodes.Status400BadRequest, "Tài Khoản của bạn đã bị xóa!", null);
            }
            if (!checkPass)
            {
                return _responseTokenObject.ResponseFail(StatusCodes.Status400BadRequest, "Password không chính xác! T.T", null);
            }
            return _responseTokenObject.ResponseSucess("Đăng Nhập Thành Công ^^!", GenerateAccessToken(user));
        }
    }
}
