using Microsoft.EntityFrameworkCore;
using WebXemPhim.Entities;
using WebXemPhim.Handle.HandlePagination;
using WebXemPhim.Handle.HandleImages;
using WebXemPhim.Payloads.Converters;
using WebXemPhim.Payloads.DataRequests;
using WebXemPhim.Payloads.DataResponses;
using WebXemPhim.Payloads.Responses;
using WebXemPhim.Services.Interfaces;

namespace WebXemPhim.Services.Implements
{
    public class BannerServices : BaseServices, IBannerServices
    {
        private readonly ResponseObject<DataResponsesBanner> _responseObject;
        private readonly BannerConverter _converter;
        public BannerServices(ResponseObject<DataResponsesBanner> _responseObject, BannerConverter _converter)
        {
            this._responseObject = _responseObject;
            this._converter = _converter;
        }
        public async Task<ResponseObject<DataResponsesBanner>> CreateBanner(Requests_CreateBanner request)
        {
            var uploadTasks = new Task<string>[]
            {
                HandleUploadFileImages.UploadLoadFile(request.ImageUrl)
            };
            var uploadResult = await Task.WhenAll(uploadTasks);
            Banner banner = new Banner
            {
                ImageUrl = uploadResult[0],
                Title = request.Title
            };
            await _appDbContext.Banners.AddAsync(banner);
            await _appDbContext.SaveChangesAsync();
            return _responseObject.ResponseSucess("Thêm banner thành công", _converter.ConvertDt(banner));
        }

        public async Task<string> DeleteBanner(int bannerId)
        {
            var banner = await _appDbContext.Banners.SingleOrDefaultAsync(x => x.Id == bannerId);
            if (banner == null)
            {
                return "Không Tồn Tại Id Banner";
            }
            _appDbContext.Banners.Remove(banner);
            await _appDbContext.SaveChangesAsync();
            return "Xóa Banner Thành Công!";
        }

        public async Task<PageResult<DataResponsesBanner>> GetAllBanners(int pageSize, int pageNumber)
        {
            var query = _appDbContext.Banners.Select(x => _converter.ConvertDt(x));
            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;
        }
        public IEnumerable<Banner> GetAllBannersNoPagination()
        {
            return _appDbContext.Banners.AsQueryable();
        }



        public async Task<ResponseObject<DataResponsesBanner>> GetBannerById(int bannerId)
        {
            var result = await _appDbContext.Banners.SingleOrDefaultAsync(x => x.Id == bannerId);
            if (result == null)
            {
                return _responseObject.ResponseFail(StatusCodes.Status404NotFound, "Không tìm thấy banner Id", null);
            }
            return _responseObject.ResponseSucess("Lấy dữ liệu thành công", _converter.ConvertDt(result));
        }

        public async Task<ResponseObject<DataResponsesBanner>> UpdateBanner(Requests_UpdateBanner request)
        {
            var banner = await _appDbContext.Banners.SingleOrDefaultAsync(x => x.Id == request.BannerId);
            if (banner == null)
            {
                return _responseObject.ResponseFail(StatusCodes.Status404NotFound, "Không Tìm Thấy ID Banner", null);
            }
            banner.Title = request.Title;
            banner.ImageUrl = await HandleUploadFileImages.UpdateFile(banner.ImageUrl, request.ImageUrl);
            _appDbContext.Banners.Update(banner);
            await _appDbContext.SaveChangesAsync();
            return _responseObject.ResponseSucess("Cập Nhật Banner Thành Công", _converter.ConvertDt(banner));
        }

        public async Task<ResponseObject<DataResponsesBanner>> UpdateBannerHaveString(Requests_UpdateBannerhavestring request)
        {
            var banner = await _appDbContext.Banners.SingleOrDefaultAsync(x => x.Id == request.BannerId);
            if (banner == null)
            {
                return _responseObject.ResponseFail(StatusCodes.Status404NotFound, "Không Tìm Thấy ID Banner", null);
            }
            banner.Title = request.Title;
            banner.ImageUrl = request.ImageUrl;
            _appDbContext.Banners.Update(banner);
            await _appDbContext.SaveChangesAsync();
            return _responseObject.ResponseSucess("Cập Nhật Banner Thành Công!!", _converter.ConvertDt(banner));
        }
    }
}
