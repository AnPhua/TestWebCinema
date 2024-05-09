using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System;
using WebXemPhim.Entities;
using WebXemPhim.Handle.HandleImages;
using WebXemPhim.Handle.HandlePagination;
using WebXemPhim.Payloads.Converters;
using WebXemPhim.Payloads.DataRequests;
using WebXemPhim.Payloads.DataResponses;
using WebXemPhim.Payloads.Responses;
using WebXemPhim.Services.Interfaces;

namespace WebXemPhim.Services.Implements
{
    public class FoodServices : BaseServices, IFoodServices
    {
        private readonly ResponseObject<DataResponsesFood> _responseObjectFood;
        private readonly FoodConverter _foodconverter;
        public FoodServices()
        {
            _foodconverter = new FoodConverter();
            _responseObjectFood = new ResponseObject<DataResponsesFood>();
        }
        public  async Task<ResponseObject<DataResponsesFood>> CreateFood(Requests_CreateFood requests)
        {
            if (requests.Price == null ||
               requests.Image == null || requests.Image.Length == 0 ||
               string.IsNullOrEmpty(requests.Description) ||
               string.IsNullOrEmpty(requests.NameOfFood))
            {
                return _responseObjectFood.ResponseFail(StatusCodes.Status400BadRequest, "Chưa Điền Đầy Đủ Thông Tin!!", null);
            }
            Food createfood = new Food
            {
                Price = requests.Price,
                Image = await HandleUploadFileImages.UploadLoadFile(requests.Image),
                Description = requests.Description,
                NameOfFood = requests.NameOfFood,
                IsActive = true,
            };
            _appDbContext.Foods.Add(createfood);
            _appDbContext.SaveChanges();
            return _responseObjectFood.ResponseSucess("Thêm Thông Tin Đồ Ăn Mới Thành Công!!", _foodconverter.ConvertDt(createfood));
        }

        public string DeleteFood(int foodId)
        {
            var deletefood =  _appDbContext.Foods.SingleOrDefault(x => x.Id == foodId);
            if (deletefood == null)
            {
                return "Không Tìm Thấy Id Đồ Ăn!!";
            }
            deletefood.IsActive = false;
            _appDbContext.Foods.Update(deletefood);
            _appDbContext.SaveChanges();
            return "Xóa Đồ Ăn Thành Công!!";
        }

        public async Task<PageResult<DataResponsesFood>> GetAllFood(int pageSize, int pageNumber)
        {
            var query = _appDbContext.Foods.Where(x => x.IsActive == true).Select(x => _foodconverter.ConvertDt(x));
            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;
        }

        public async Task<ResponseObject<DataResponsesFood>> GetFoodById(int foodId)
        {
            var result = await _appDbContext.Foods.SingleOrDefaultAsync(x => x.Id == foodId);
            if (result == null)
            {
                return _responseObjectFood.ResponseFail(StatusCodes.Status404NotFound, "Không Tìm Thấy Food Id", null);
            }
            return _responseObjectFood.ResponseSucess("Lấy dữ liệu thành công", _foodconverter.ConvertDt(result));
        }

        public async Task<ResponseObject<DataResponsesFood>> UpdateFood(Requests_UpdateFood requests)
        {
            var foodupdate = await _appDbContext.Foods.SingleOrDefaultAsync(x => x.Id == requests.FoodId);
            if (foodupdate == null)
            {
                return _responseObjectFood.ResponseFail(StatusCodes.Status404NotFound, "Không Tồn Tại Id Đồ Ăn", null);
            }
            foodupdate.Description = requests.Description;
            foodupdate.Price = requests.Price;
            foodupdate.NameOfFood = requests.NameOfFood;
            foodupdate.Image = await HandleUploadFileImages.UpdateFile(foodupdate.Image, requests.Image);
            _appDbContext.Foods.Update(foodupdate);
            await _appDbContext.SaveChangesAsync();
            return _responseObjectFood.ResponseSucess("Cập Nhật Thông Tin Đồ Ăn Thành Công", _foodconverter.ConvertDt(foodupdate));
        }
        public async Task<ResponseObject<DataResponsesFood>> UpdateFoodHaveString(Requests_UpdateFoodhavestring requests)
        {
            var foodupdate = await _appDbContext.Foods.SingleOrDefaultAsync(x => x.Id == requests.FoodId);
            if (foodupdate == null)
            {
                return _responseObjectFood.ResponseFail(StatusCodes.Status404NotFound, "Không Tồn Tại Id Đồ Ăn", null);
            }
            foodupdate.Description = requests.Description;
            foodupdate.Price = requests.Price;
            foodupdate.NameOfFood = requests.NameOfFood;
            foodupdate.Image = requests.Image;
            _appDbContext.Foods.Update(foodupdate);
            await _appDbContext.SaveChangesAsync();
            return _responseObjectFood.ResponseSucess("Cập Nhật Thông Tin Đồ Ăn Thành Công", _foodconverter.ConvertDt(foodupdate));
        }
    }
}
