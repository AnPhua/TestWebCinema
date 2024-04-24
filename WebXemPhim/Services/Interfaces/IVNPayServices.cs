namespace WebXemPhim.Services.Interfaces
{
    public interface IVNPayServices
    {
        Task<string> CreatePaymentUrl(int billId, HttpContext httpContext, int id);
        Task<string> VNPayReturn(IQueryCollection vnpayData);
    }
}
