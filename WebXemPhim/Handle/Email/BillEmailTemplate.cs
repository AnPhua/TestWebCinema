using WebXemPhim.Entities;
using System.Linq;

namespace WebXemPhim.Handle.Email
{
    public class BillEmailTemplate
    {
        public static string GenerateNotificationBillEmail(Bill bill, string message = "")
        {
            AppDbContext context = new AppDbContext();

            var billTickets = context.BillTickets.Where(bt => bt.BillId == bill.Id)
                             .Select(bt => new
                             {
                                 SeatName = bt.Ticket.Seat.Line + bt.Ticket.Seat.Number,
                                 RoomName = bt.Ticket.Schedule.Room.Name,
                                 StartAt = bt.Ticket.Schedule.StartAt.ToString("HH:mm 'Giờ - Ngày' dd-MM-yyyy"),
                                 MovieName = bt.Ticket.Schedule.Movie.Name,
                                 bt.Ticket.PriceTicket
                             }).ToList();

            var billFoods = context.BillFoods.Where(bf => bf.BillId == bill.Id)
                             .Select(bf => new
                             {
                                 bf.Food.NameOfFood,
                                 bf.Food.Price,
                                 bf.Quantity
                             }).ToList();

            string htmlContent = $@"
    <html>
    <head>
        <style>
            body {{
                font-family: Arial, sans-serif;
            }}
            img {{
                width: 100%;
                height: 10%;
            }}
            h1 {{
                color: #333;
            }}
            table {{
                border-collapse: collapse;
                width: 100%;
            }}
            th, td {{
                border: 1px solid #ddd;
                padding: 8px;
            }}
            th {{
                background-color: #f2f2f2;
                font-weight: bold;
            }}
            .footer {{
                margin-top: 20px;
                font-size: 14px;
            }}
        </style>
    </head>
    <body>
        <img src=""https://i.pinimg.com/564x/85/a4/4b/85a44b48f70d9f421e014b7b3a47e42b.jpg"" alt=""Invoice Image"">
        <h1>Thông tin hóa đơn</h1>
        <h2 style=""color: red; font-size: 20px; font-weight: bold;"">{(string.IsNullOrEmpty(message) ? "" : message)}</h2>

        <table>
            <tr>
                <th>Mã giao dịch</th>
                <th>Tên hóa đơn</th>
                <th>Tổng tiền</th>
                <th>Trạng thái hóa đơn</th>
                <th>Tên khách hàng</th>
                <th>Ngày tạo</th>
            </tr>
            <tr>
                <td style=""text-align: center;"">{bill.TradingCode}</td>
                <td style=""text-align: center;"">{bill.Name}</td>
                <td style=""text-align: center;"">{bill.TotalMoney?.ToString("#,##0")} VNĐ</td>
                <td style=""text-align: center;"">{context.BillStatuses.SingleOrDefault(x => x.Id == bill.BillStatusId).Name}</td>
                <td style=""text-align: center;"">{context.Users.SingleOrDefault(x => x.Id == bill.CustomerId).Name}</td>
                <td style=""text-align: center;"">{bill.CreateTime}</td>
            </tr>
        </table>

        <h2 style=""color: red; font-size: 20px; font-weight: bold;"">Thông tin vé, đồ ăn</h2>
        <table>
            <tr>
                <th>Ghế</th>
                <th>Phòng Chiếu</th>
                <th>Giờ Chiếu</th>
                <th>Tên Phim</th>
                <th>Giá Vé</th>
                <th>Số Lượng</th>
            </tr>";

            foreach (var ticket in billTickets)
            {
                htmlContent += $@"
            <tr>
                <td style=""text-align: center;"">{ticket.SeatName}</td>
                <td style=""text-align: center;"">{ticket.RoomName}</td>
                <td style=""text-align: center;"">{ticket.StartAt}</td>
                <td style=""text-align: center;"">{ticket.MovieName}</td>
                <td style=""text-align: center;"">{ticket.PriceTicket.ToString("#,##0")} VNĐ</td>
                <td style=""text-align: center;"">1</td>
            </tr>";
            }

            htmlContent += $@"
        </table>

        <table>
            <tr>
                <th>Tên đồ ăn</th>
                <th>Giá đồ ăn</th>
                <th>Số lượng</th>
            </tr>";

            foreach (var food in billFoods)
            {
                htmlContent += $@"
            <tr>
                <td style=""text-align: center;"">{food.NameOfFood}</td>
                <td style=""text-align: center;"">{food.Price.ToString("#,##0")} VNĐ</td>
                <td style=""text-align: center;"">{food.Quantity}</td>
            </tr>";
            }

            htmlContent += $@"
        </table>
        <div class=""footer"">
            <p>Thank You^^-AnAn</p>
        </div>
    </body>
    </html>";

            return htmlContent;
        }

    }
}
