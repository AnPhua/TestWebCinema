using Microsoft.EntityFrameworkCore;
using WebXemPhim.Entities;
using WebXemPhim.Handle.HandlePagination;
using WebXemPhim.Payloads.Converters;
using WebXemPhim.Payloads.DataRequests;
using WebXemPhim.Payloads.DataResponses;
using WebXemPhim.Payloads.Responses;
using WebXemPhim.Services.Interfaces;
using WebXemPhim.Handle.Generate;
using WebXemPhim.Services.Implements;
using System.Data;

namespace WebXemPhim.Services.Implements
{
    public class BillServices :BaseServices,IBillServices
    {
        private readonly ResponseObject<DataResponsesBillFood> _responseBillFoodObject;
        private readonly ResponseObject<DataResponsesBillTicket> _responseBillTicketObject;
        private readonly ResponseObject<DataResponsesBill> _responseObject;
        private readonly BillConverter _billConverter;
        private readonly BillTicketConverter _billTicketConverter;
        private readonly BillFoodConverter _billFoodConverter;
        public BillServices(ResponseObject<DataResponsesBillFood> responseBillFoodObject, ResponseObject<DataResponsesBillTicket> responseBillTicketObject, ResponseObject<DataResponsesBill> responseObject, BillConverter billConverter, BillTicketConverter billTicketConverter, BillFoodConverter billFoodConverter)
        {
            _responseBillFoodObject = responseBillFoodObject;
            _responseBillTicketObject = responseBillTicketObject;
            _responseObject = responseObject;
            _billConverter = billConverter;
            _billTicketConverter = billTicketConverter;
            _billFoodConverter = billFoodConverter;
        }

        //public async Task<ResponseObject<DataResponseBill>> CreateBill(Request_CreateBill request)
        //{
        //    var customer = await _appDbContext.users.SingleOrDefaultAsync(x => x.Id == request.CustomerId);
        //    if(customer == null)
        //    {
        //        return _responseObject.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy thông tin khách hàng", null);
        //    }
        //    var promotion = await _appDbContext.promotions.SingleOrDefaultAsync(x => x.Id == request.PromotionId);
        //    var bill = new Bill();
        //    bill.CustomerId = request.CustomerId;
        //    bill.TradingCode = GenerateCode.GenerateBillCode();
        //    bill.CreateAt = DateTime.Now;
        //    bill.CreateTime = DateTime.Now;
        //    bill.Name = request.BillName;
        //    bill.BillStatusId = 1;
        //    bill.PromotionId = request.PromotionId == null ? null : request.PromotionId;
        //    bill.BillTickets = null;
        //    bill.BillFoods = null;
        //    bill.IsActive = true;
        //    bill.TotalMoney = 0;
        //    await _appDbContext.bills.AddAsync(bill);
        //    await _appDbContext.SaveChangesAsync();
        //    bill.BillTickets = await CreateListBillTicket(bill.Id, request.BillTickets);
        //    bill.BillFoods = request.BillFoods != null ? await CreateListBillFood(bill.Id, request.BillFoods) : null;
        //    double priceTicket = 0;
        //    double priceFood = 0;
        //    bill.BillTickets.ForEach(x =>
        //    {
        //        var ticket = _appDbContext.tickets.SingleOrDefault(y => y.Id == x.TicketId);
        //        priceTicket += ticket.PriceTicket * x.Quantity;
        //    });
        //    bill.BillFoods?.ForEach(x =>
        //    {
        //        var food = _appDbContext.foods.SingleOrDefault(y => y.Id == x.FoodId);
        //        priceFood += food.Price * x.Quantity;
        //    });
        //    if(request.BillFoods == null && request.PromotionId != null)
        //    {
        //        bill.TotalMoney = priceTicket -  ((priceTicket * promotion.Percent * 1.0)/100);
        //    }
        //    else if(request.BillFoods == null && request.PromotionId == null)
        //    {
        //        bill.TotalMoney = priceTicket;
        //    }
        //    bill.TotalMoney = (priceTicket + priceFood) - (((priceTicket + priceFood) * promotion?.Percent) * 1.0 / 100);
        //    _appDbContext.bills.Update(bill);
        //    await _appDbContext.SaveChangesAsync();
        //    return _responseObject.ResponseSuccess("Tạo hóa đơn thành công", _billConverter.EntityToDTO(bill));

        //}
        public async Task<ResponseObject<DataResponsesBill>> CreateBill(Requests_CreateBill request)
        {
            var customer = await _appDbContext.Users.SingleOrDefaultAsync(x => x.Id == request.CustomerId);
            if (customer == null)
            {
                return _responseObject.ResponseFail(StatusCodes.Status404NotFound, "Không Tìm Thấy Thông Tin Khách Hàng", null);
            }

            var promotion = await _appDbContext.Promotions.SingleOrDefaultAsync(x => x.Id == request.PromotionId);
            var existingTickets = await _appDbContext.BillTickets.Where(x => request.BillTickets.Select(bt => bt.TicketId).Contains(x.TicketId)).ToListAsync();
            if (existingTickets.Any())
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Vé Đã Được Mua Bởi Người Khác", null);
            }
            Bill bill = new Bill
            {
                CustomerId = request.CustomerId == 0 ? 1013 : request.CustomerId,
                TradingCode = GenerateCode.GenerateCodes(),
                CreateTime = DateTime.Now,
                Name = "Bill"  + new Random().Next(100, 999).ToString() + DateTime.Now.Ticks.ToString(),
                BillStatusId = 1,
                PromotionId = request.PromotionId == 0 ? 1 : request.PromotionId,
                BillTickets = null,
                BillFoods = null,
                IsActive = true,
                TotalMoney = 0
            };

            await _appDbContext.Bills.AddAsync(bill);
            await _appDbContext.SaveChangesAsync();

            bill.BillTickets = await CreateListBillTicket(bill.Id, request.BillTickets);
            bill.BillFoods = request.BillFoods != null ? await CreateListBillFood(bill.Id, request.BillFoods) : null;

            double priceTicket = bill.BillTickets?.Sum(x => _appDbContext.Tickets.SingleOrDefault(y => y.Id == x.TicketId).PriceTicket * x.Quantity) ?? 0;
            double priceFood = bill.BillFoods?.Sum(x => _appDbContext.Foods.SingleOrDefault(y => y.Id == x.FoodId).Price * x.Quantity) ?? 0;
            double total = priceTicket + priceFood;

            if (promotion != null)
            {
                bill.TotalMoney = total - (total * promotion.Percent / 100.0);
            }
            else
            {
                bill.TotalMoney = total;
            }

            _appDbContext.Bills.Update(bill);
            await _appDbContext.SaveChangesAsync();

            return _responseObject.ResponseSucess("Tạo hóa đơn thành công", _billConverter.ConvertDt(bill));
        }

        public async Task<ResponseObject<DataResponsesBillFood>> CreateBillFood(int billId, Requests_CreateBillFood request)
        {
            var bill = await _appDbContext.Bills.SingleOrDefaultAsync(x => x.Id == billId);
            if (bill == null)
            {
                return _responseBillFoodObject.ResponseFail(StatusCodes.Status404NotFound, "Hóa đơn không tồn tại", null);
            }
            var billFood = new BillFood
            {
                BillId = billId,
                Quantity = request.Quantity,
                FoodId = request.FoodId,
            };
            await _appDbContext.BillFoods.AddAsync(billFood);
            await _appDbContext.SaveChangesAsync();
            return _responseBillFoodObject.ResponseSucess("Thêm bill food thành công", _billFoodConverter.ConvertDt(billFood));
        }

        public async Task<ResponseObject<DataResponsesBillTicket>> CreateBillTicket(int billId, Requests_CreateBillTicket request)
        {
            var bill = await _appDbContext.Bills.Include(x => x.Promotion).AsNoTracking().SingleOrDefaultAsync(x => x.Id == billId);
            if (bill == null)
            {
                return _responseBillTicketObject.ResponseFail(StatusCodes.Status404NotFound, "Hóa đơn không tồn tại", null);
            }
            var billTicket = new BillTicket
            {
                BillId = billId,
                Quantity = 1,
                TicketId = request.TicketId,
            };
            await _appDbContext.BillTickets.AddAsync(billTicket);
            await _appDbContext.SaveChangesAsync();
            return _responseBillTicketObject.ResponseSucess("Thêm bill ticket thành công", _billTicketConverter.ConvertDt(billTicket));
        }

        public async Task<List<BillFood>> CreateListBillFood(int billId, List<Requests_CreateBillFood> requests)
        {
            var bill = await _appDbContext.Bills.SingleOrDefaultAsync(x => x.Id == billId);
            if (bill == null)
            {
                return null;
            }
            List<BillFood> list = new List<BillFood>();
            foreach (Requests_CreateBillFood request in requests)
            {
                BillFood billTicket = new BillFood
                {
                    BillId = billId,
                    Quantity = request.Quantity,
                    FoodId = request.FoodId,
                };
                list.Add(billTicket);
            }
            await _appDbContext.BillFoods.AddRangeAsync(list);
            await _appDbContext.SaveChangesAsync();
            return list;
        }

        public async Task<List<BillTicket>> CreateListBillTicket(int billId, List<Requests_CreateBillTicket> requests)
        {
            var bill = await _appDbContext.Bills.SingleOrDefaultAsync(x => x.Id == billId);
            if(bill == null)
            {
                return null;
            }
            List<BillTicket> list = new List<BillTicket>();
            foreach(Requests_CreateBillTicket request in requests)
            {
                BillTicket billTicket = new BillTicket
                {
                    BillId = billId,
                    Quantity = 1,
                    TicketId = request.TicketId,
                };
                list.Add(billTicket);
            }
            await _appDbContext.BillTickets.AddRangeAsync(list);
            await _appDbContext.SaveChangesAsync();
            return list;
        }

        public async Task<ResponseObject<DataResponsesBill>> GetPaymentHistoryByBillId(int billId)
        {
            var result = await _appDbContext.Bills.Include(x => x.BillFoods).Include(x => x.BillTickets).AsNoTracking().SingleOrDefaultAsync(x => x.Id == billId);
            if(result.BillStatusId == 1)
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Hóa đơn vẫn chưa được thanh toán", null);
            }
            return _responseObject.ResponseSucess("Thông tin thanh toán của hóa đơn", _billConverter.ConvertDt(result));
        }

        public async Task<PageResult<DataResponsesBill>> GetAllBills(int pageSize, int pageNumber)
        {
            var query = _appDbContext.Bills.Include(x => x.BillTickets).Include(x => x.BillFoods).Where(x => x.BillStatusId == 2).Select(x => _billConverter.ConvertDt(x));
            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;
        }
        public async Task<IQueryable<DataStatisticSales>> SalesStatistics(InputStatistic input)
        {
            var query = _appDbContext.Bills.Include(x => x.BillFoods).ThenInclude(x => x.Food)
                                      .Include(x => x.BillTickets).ThenInclude(x => x.Ticket).ThenInclude(x => x.Schedule).ThenInclude(x => x.Room).ThenInclude(x => x.Cinema)
                                      .AsNoTracking()
                                      .Where(x => x.BillStatusId == 2);
            if (input.CinemaId.HasValue)
            {
                query = query.Where(x => x.BillTickets.Any(y => y.Ticket.Schedule.Room.CinemaId == input.CinemaId));
            }

            var billStats = await query
                .GroupBy(x => new
                {
                    CinemaId = input.CinemaId.HasValue ? x.BillTickets.FirstOrDefault().Ticket.Schedule.Room.CinemaId : (int?)null
                })
                .Select(group => new DataStatisticSales
                {
                    CinemaId = group.Key.CinemaId,
                    Sales = group.Sum(item => item.TotalMoney),
                })
                .ToListAsync();

            return billStats.AsQueryable();
        }
        //public async Task<IQueryable<DataStatisticsFood>> SalesStatisticsFood(InputFoodStatistics input)
        //{
        //    var query = _appDbContext.bills.Include(x => x.BillFoods).ThenInclude(x => x.Food)
        //                              .AsNoTracking()
        //                              .Where(x => x.BillStatusId == 2);

        //    if (input.FoodId.HasValue)
        //    {
        //        query = query.Where(x => x.BillFoods.Any(y => y.FoodId == input.FoodId));
        //    }

        //    if (input.StartAt.HasValue)
        //    {
        //        query = query.Where(x => x.CreateAt.Date >= input.StartAt.Value.Date);
        //    }

        //    if (input.EndAt.HasValue)
        //    {
        //        query = query.Where(x => x.CreateAt.Date <= input.EndAt.Value.Date);
        //    }

        //    var billFoodStats = await query
        //                               .SelectMany(x => x.BillFoods)
        //                               .Where(bf => !input.FoodId.HasValue || bf.FoodId == input.FoodId)
        //                               .GroupBy(bf => bf.FoodId)
        //                               .Select(group => new DataStatisticsFood
        //                               {
        //                                   FoodId = group.Key,
        //                                   Sales = group.Sum(x => x.Quantity * (x.Food.Price)),
        //                                   SellNumber = group.Count()
        //                               }).ToListAsync();

        //    return billFoodStats.AsQueryable();
        //}
        public async Task<IQueryable<DataStatisticsFood>> SalesStatisticsFood(InputFoodStatistics input)
        {
            var query = _appDbContext.Bills.Include(x => x.BillFoods).ThenInclude(x => x.Food)
                                      .AsNoTracking()
                                      .Where(x => x.BillStatusId == 2);

            if (input.FoodId.HasValue)
            {
                query = query.Where(x => x.BillFoods.Any(y => y.FoodId == input.FoodId));
            }

            var billFoodStats = await query
                .SelectMany(x => x.BillFoods)
                .Where(bf => !input.FoodId.HasValue || bf.FoodId == input.FoodId)
                .GroupBy(bf => new
                {
                    FoodId = bf.FoodId
                })
                .Select(group => new DataStatisticsFood
                {
                    FoodId = group.Key.FoodId,
                    Sales = group.Sum(x => x.Quantity * x.Food.Price),
                    SellNumber = group.Count()
                }).ToListAsync();

            return billFoodStats.AsQueryable();
        }

    }
}
