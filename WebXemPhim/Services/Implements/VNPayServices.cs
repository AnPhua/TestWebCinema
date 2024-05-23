using Microsoft.EntityFrameworkCore;
using WebXemPhim.Entities;
using WebXemPhim.Handle.Email;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebXemPhim.Handle.VNPay;
using WebXemPhim.Services.Interfaces;

namespace WebXemPhim.Services.Implements
{
    public class VNPayServices:BaseServices,IVNPayServices
    {
        private readonly IConfiguration _configuration;
        private readonly VNPayLibrary pay;
        private readonly AuthServices _authService;
        public VNPayServices(IConfiguration configuration, AuthServices authService)
        {
            _configuration = configuration;
            pay = new VNPayLibrary();
            _authService = authService;
        }
        public async Task<string> CreatePaymentUrl(int billId, HttpContext httpContext, int id)
        {
            var bill = await _appDbContext.Bills.SingleOrDefaultAsync(x => x.Id == billId);
            var user = await _appDbContext.Users.SingleOrDefaultAsync(x => x.Id == id);
            if (user.Id == bill.CustomerId)
            {
                if (bill.BillStatusId == 2)
                {
                    return "Hóa đơn đã được thanh toán trước đó";
                }
                if (bill.BillStatusId == 1 && bill.TotalMoney != 0 && bill.TotalMoney != null)
                {
                    pay.AddRequestData("vnp_Version", "2.1.0");
                    pay.AddRequestData("vnp_Command", "pay");
                    pay.AddRequestData("vnp_TmnCode", "R63HC2N5");
                    pay.AddRequestData("vnp_Amount", (bill.TotalMoney*100).ToString());
                    pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    pay.AddRequestData("vnp_CurrCode", "VND");
                    pay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(httpContext));
                    pay.AddRequestData("vnp_Locale", "vn");
                    pay.AddRequestData("vnp_OrderInfo", $"Thanh toán hóa đơn{billId}");
                    pay.AddRequestData("vnp_OrderType", "other");
                    pay.AddRequestData("vnp_ReturnUrl", _configuration.GetSection("VnPay:vnp_ReturnUrl").Value);
                    pay.AddRequestData("vnp_TxnRef", billId.ToString());

                    string paymentUrl = pay.CreateRequestUrl(_configuration.GetSection("VnPay:vnp_Url").Value, _configuration.GetSection("VnPay:vnp_HashSecret").Value);
                    return paymentUrl;
                }
                else
                {
                    return "Vui lòng kiểm tra lại hóa đơn";
                }
            }
            return "Vui lòng kiểm tra lại hóa đơn";
        }
        public async Task<string> VNPayReturn(IQueryCollection vnpayData)
        {
            string vnp_TmnCode = _configuration.GetSection("VnPay:vnp_TmnCode").Value;
            string vnp_HashSecret = _configuration.GetSection("VnPay:vnp_HashSecret").Value;

            VNPayLibrary vnPayLibrary = new VNPayLibrary();
            foreach (var (key, value) in vnpayData)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnPayLibrary.AddResponseData(key, value);
                }
            }

            string billId = vnPayLibrary.GetResponseData("vnp_TxnRef");
            string vnp_ResponseCode = vnPayLibrary.GetResponseData("vnp_ResponseCode");
            string vnp_TransactionStatus = vnPayLibrary.GetResponseData("vnp_TransactionStatus");
            string vnp_SecureHash = vnPayLibrary.GetResponseData("vnp_SecureHash");
            double vnp_Amount = Convert.ToDouble(vnPayLibrary.GetResponseData("vnp_Amount"));
            bool check = vnPayLibrary.ValidateSignature(vnp_SecureHash, vnp_HashSecret);

            if (check)
            {/**/
                if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                {
                    var bill = await _appDbContext.Bills.Include(x => x.BillTickets)
                                                   .ThenInclude(x => x.Ticket)
                                                   .ThenInclude(x => x.Seat)
                                                   .FirstOrDefaultAsync(x => x.Id == Convert.ToInt32(billId));

                    if (bill == null)
                    {
                        return "Không tìm thấy hóa đơn";
                    }

                    bill.BillStatusId = 2;
                    bill.CreateTime = DateTime.Now;

                    var billTicket = bill.BillTickets.Where(x => x.BillId == bill.Id).ToList();
                    foreach (var item in billTicket)
                    {
                        item.Ticket.IsActive = false;
                        item.Ticket.TypeTicket = 5;
                        item.Ticket.Seat.SeatStatusId = 1;
                        _appDbContext.Seats.Update(item.Ticket.Seat);
                    }

                    _appDbContext.Bills.Update(bill);
                    await _appDbContext.SaveChangesAsync();

                    var user = _appDbContext.Users.FirstOrDefault(x => x.Id == bill.CustomerId);
                    if (user != null)
                    {
                        user.Point += 100;
                        var rankCustomers = await _appDbContext.RankCustomers.ToListAsync();
                        var newRank = rankCustomers
                            .Where(r => user.Point >= r.Point)
                            .OrderByDescending(r => r.Point)
                            .FirstOrDefault();

                        if (newRank != null)
                        {
                            user.RankCustomerId = newRank.Id;
                        }
                        _appDbContext.Users.Update(user);
                        string email = user.Email;
                        string mss = _authService.SendMail(new SendEmail
                        {
                            Email = email,
                            Title = $"Thanh Toán đơn hàng: {bill.Id}",
                            Body = BillEmailTemplate.GenerateNotificationBillEmail(bill, "THANH TOÁN")
                        });
                    }
                    return "Đơn hàng " + bill.Id + " đã được thanh toán thành công!";

                }
                else
                {
                    return "Có lỗi xảy ra trong quá trình thanh toán. Mã lỗi: " + vnp_ResponseCode;
                }
            }
            else
            {
                return "Có lỗi trong quá trình xử lý";
            }
        }
    }
}
