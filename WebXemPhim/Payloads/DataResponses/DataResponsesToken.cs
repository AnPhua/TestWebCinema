namespace WebXemPhim.Payloads.DataResponses
{
    public class DataResponsesToken
    {
        public string AccessToken { get; set; } // Khi Đăng Nhập sẽ trả về access token.Acesstoken dùng để giải mã thông tin có thể để tùy theo tgian mình đặt
        public string RefreshToken { get; set; } // dùng để reset token sinh ra token mới để tránh bị log ra lưu trong database , tgian sẽ dài hơn accesstoken để đc reset
        public DataResponsesUser DataResponseUser { get; set; }
    }
}
