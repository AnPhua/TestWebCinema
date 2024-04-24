using WebXemPhim.Entities;
using WebXemPhim.Handle.HandlePagination;
using WebXemPhim.Payloads.DataRequests;
using WebXemPhim.Payloads.DataResponses;
using WebXemPhim.Payloads.Responses;

namespace WebXemPhim.Services.Interfaces
{
    public interface IBillServices
    {
        Task<ResponseObject<DataResponsesBillTicket>> CreateBillTicket(int billId, Requests_CreateBillTicket request);
        Task<List<BillTicket>> CreateListBillTicket(int billId, List<Requests_CreateBillTicket> requests);
        Task<ResponseObject<DataResponsesBillFood>> CreateBillFood(int billId, Requests_CreateBillFood request);
        Task<List<BillFood>> CreateListBillFood(int billId, List<Requests_CreateBillFood> requests);
        Task<ResponseObject<DataResponsesBill>> CreateBill(Requests_CreateBill request);
        Task<ResponseObject<DataResponsesBill>> GetPaymentHistoryByBillId(int billId);
        Task<PageResult<DataResponsesBill>> GetAllBills(int pageSize, int pageNumber);
        Task<IQueryable<DataStatisticSales>> SalesStatistics(InputStatistic input);
        Task<IQueryable<DataStatisticsFood>> SalesStatisticsFood(InputFoodStatistics input);
    }
}
