﻿using Azure.Core;
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
    public class MovieServices : BaseServices,IMovieServices
    {
        private readonly ResponseObject<DataResponsesMovie> _responseObjectMovie;
        private readonly ResponseObject<DataResponsesMovieType> _responseObjectMovieType;
        private readonly ResponseObject<DataResponsesRate> _responseObjectRate;
        private readonly MovieConverter _movieConverter;
        private readonly MovieTypeConverter _movieTypeConverter;     
        private readonly RateConverter _rateConverter;
        public MovieServices()
        {
            _responseObjectMovie = new ResponseObject<DataResponsesMovie>();
            _responseObjectMovieType = new ResponseObject<DataResponsesMovieType>();
            _responseObjectRate = new ResponseObject<DataResponsesRate>();
            _movieConverter = new MovieConverter();
            _movieTypeConverter = new MovieTypeConverter();
            _rateConverter = new RateConverter();
        }
        public async Task<ResponseObject<DataResponsesMovie>> CreateMovie(Requests_CreateMovie requests)
        {
            if(requests.MovieDuration == null ||
               requests.EndTime == null ||
               requests.PremiereDate == null || 
               string.IsNullOrEmpty(requests.Description) ||
               string.IsNullOrEmpty(requests.Director) ||
               string.IsNullOrEmpty(requests.Name)||
               requests.Image == null || requests.Image.Length == 0 ||
               requests.HeroImage == null || requests.HeroImage.Length == 0 ||
               string.IsNullOrEmpty(requests.Language) ||
               requests.MovieTypeId == null || 
               requests.RateId == null ||
               string.IsNullOrEmpty(requests.Trailer))
            {
                return _responseObjectMovie.ResponseFail(StatusCodes.Status400BadRequest, "Chưa Điền Đầy Đủ Thông Tin!!", null);
            }
            var movieType = _appDbContext.MovieTypes.Find(requests.MovieTypeId);
            var rate = _appDbContext.Rates.Find(requests.RateId);
            if (movieType == null || rate == null)
            {
                return _responseObjectMovie.ResponseFail(StatusCodes.Status404NotFound, "Không Tìm Thấy Thể Loại Phim Hoặc Rate Phim!!", null);
            }
            var uploadTasks = new Task<string>[]
            {
                HandleUploadFileImages.UploadLoadFile(requests.Image),
                HandleUploadFileImages.UploadLoadFile(requests.HeroImage)
            };
            var uploadResult = await Task.WhenAll(uploadTasks);
            Movie movie = new Movie
            {
                Description = requests.Description,
                Director = requests.Director,
                EndTime = requests.EndTime,
                Image = uploadResult[0],
                HeroImage = uploadResult[1],
                Language = requests.Language,
                MovieDuration = requests.MovieDuration,
                MovieTypeId = requests.MovieTypeId,
                Name = requests.Name,
                PremiereDate = requests.PremiereDate,
                RateId = requests.RateId,
                Trailer = requests.Trailer
            };
            _appDbContext.Movies.Add(movie);
            _appDbContext.SaveChanges();
            return _responseObjectMovie.ResponseSucess("Thêm Thông Tin Phim Thành Công", _movieConverter.ConvertDt(movie));
        }

        public ResponseObject<DataResponsesMovieType> CreateMovieType(Requests_CreateMovieType requests)
        {
            if (string.IsNullOrWhiteSpace(requests.MovieTypeName))
            {
                return _responseObjectMovieType.ResponseFail(StatusCodes.Status400BadRequest, "Chưa Điền Đầy Đủ Thông Tin!!", null);
            }
            MovieType movieType = new MovieType
            {
                MovieTypeName = requests.MovieTypeName,
                IsActive = true
            };
            _appDbContext.MovieTypes.Add(movieType);
            _appDbContext.SaveChanges();
            return _responseObjectMovieType.ResponseSucess("Tạo Thể Loại Phim Thành Công!!", _movieTypeConverter.ConvertDt(movieType));
        }

        public ResponseObject<DataResponsesRate> CreateRate(Requests_CreateRate requests)
        {
            if (string.IsNullOrWhiteSpace(requests.Description) || string.IsNullOrWhiteSpace(requests.Code))
            {
                return _responseObjectRate.ResponseFail(StatusCodes.Status400BadRequest, "Chưa Điền Đầy Đủ Thông Tin!!", null);
            }
            Rate rate = new Rate
            {
                Description = requests.Description,
                Code = requests.Code,
            };
            _appDbContext.Rates.Add(rate);
            _appDbContext.SaveChanges();
            return _responseObjectRate.ResponseSucess("Tạo Rate Phim Thành Công!!", _rateConverter.ConvertDt(rate));
        }

        public string DeleteMovie(int movieId)
        {
            var movie =  _appDbContext.Movies.SingleOrDefault(x => x.Id == movieId);
            if (movie == null)
            {
                return "Không Tìm Thấy Id Phim!!";
            }
            movie.IsActive = false;
            _appDbContext.Movies.Update(movie);
            _appDbContext.SaveChanges();
            return "Xóa Phim Thành Công!!";
        }

        public string DeleteMovieType(int movieTypeId)
        {
            var movieType = _appDbContext.MovieTypes.SingleOrDefault(x => x.Id == movieTypeId);
            if (movieType == null)
            {
                return "Không Tìm Thấy Id Thể Loại Phim!!";
            }
            movieType.IsActive = false;
            _appDbContext.MovieTypes.Update(movieType);
            _appDbContext.SaveChanges();
            return "Xóa Thể Loại Phim Thành Công!!";
        }

        public string DeleteRate(int rateId)
        {
            var ratemovieType = _appDbContext.Rates.SingleOrDefault(x => x.Id == rateId);
            if (ratemovieType == null)
            {
                return "Không Tìm Thấy Id Rate Phim!!";
            }
            _appDbContext.Rates.Remove(ratemovieType);
            _appDbContext.SaveChanges();
            return "Xóa Rate Phim Thành Công!!";
        }

        public async Task<ResponseObject<DataResponsesMovie>> UpdateMovie(Requests_UpdateMovie requests)
        {
            var movieUpdate =  _appDbContext.Movies.SingleOrDefault(x => x.Id == requests.Id);
            if (movieUpdate is null)
            {
                return _responseObjectMovie.ResponseFail(StatusCodes.Status404NotFound, "Không Tìm Thấy Id Của Phim", null);
            }
            if (!_appDbContext.MovieTypes.Any(x => x.Id == requests.MovieTypeId))
            {
                return _responseObjectMovie.ResponseFail(StatusCodes.Status404NotFound, "Không Tìm Thấy Thể Loại Phim", null);
            }
            if (!_appDbContext.Rates.Any(x => x.Id == requests.RateId))
            {
                return _responseObjectMovie.ResponseFail(StatusCodes.Status404NotFound, "Không Tìm Thấy Rate Phim", null);
            }
            movieUpdate.Director = requests.Director;
            movieUpdate.MovieDuration = requests.MovieDuration;
            movieUpdate.Description = requests.Description;
            movieUpdate.EndTime = requests.EndTime;
            movieUpdate.Image = await HandleUploadFileImages.UpdateFile(movieUpdate.Image, requests.Image);
            movieUpdate.HeroImage = await HandleUploadFileImages.UpdateFile(movieUpdate.HeroImage, requests.HeroImage);
            movieUpdate.Language = requests.Language;
            movieUpdate.MovieTypeId = requests.MovieTypeId;
            movieUpdate.Name = requests.Name;
            movieUpdate.RateId = requests.RateId;
            movieUpdate.Trailer = requests.Trailer;
            _appDbContext.Movies.Update(movieUpdate);
            _appDbContext.SaveChanges();
            return _responseObjectMovie.ResponseSucess("Cập Nhật Thông Tin Phim Thành Công!!", _movieConverter.ConvertDt(movieUpdate));
        }

        public ResponseObject<DataResponsesMovieType> UpdateMovieType(Requests_UpdateMovieType requests)
        {
            var movieUpdateType = _appDbContext.MovieTypes.SingleOrDefault(x => x.Id == requests.Id);
            if (movieUpdateType is null)
            {
                return _responseObjectMovieType.ResponseFail(StatusCodes.Status404NotFound, "Không Tìm Thấy Id Thể Loại Phim", null);
            }
            movieUpdateType.MovieTypeName = requests.MovieTypeName;
            _appDbContext.MovieTypes.Update(movieUpdateType);
            _appDbContext.SaveChanges();
            return _responseObjectMovieType.ResponseSucess("Cập Nhật Thông Tin Thể Loại Phim Thành Công!!", _movieTypeConverter.ConvertDt(movieUpdateType));
        }

        public ResponseObject<DataResponsesRate> UpdateRate(Requests_UpdateRate requests)
        {
            var movieUpdateRate = _appDbContext.Rates.SingleOrDefault(x => x.Id == requests.Id);
            if (movieUpdateRate is null)
            {
                return _responseObjectRate.ResponseFail(StatusCodes.Status404NotFound, "Không Tìm Thấy Id Rate Phim", null);
            }
            movieUpdateRate.Description = requests.Description;
            movieUpdateRate.Code = requests.Code;
            _appDbContext.Rates.Update(movieUpdateRate);
            _appDbContext.SaveChanges();
            return _responseObjectRate.ResponseSucess("Cập Nhật Thông Tin Thể Loại Phim Thành Công!!", _rateConverter.ConvertDt(movieUpdateRate));

        }
        //Hiển thị các bộ phim nổi bật(sắp xếp theo số lượng đặt vé)
        //public PageResult<DataResponsesMovie> GetFeaturedMovies(int pageSize, int pageNumber)
        //{
        //    var query = _appDbContext.Movies
        //                .AsNoTracking()
        //                .Where(m => m.IsActive == true)
        //                .Select(m => new { Movie = m, TotalTickets = m.Schedules.Sum(s => s.Tickets.Sum(t => t.BillTickets.Sum(bt => bt.Quantity))) })
        //                .OrderByDescending(mt => mt.TotalTickets)
        //                .Select(mt => mt.Movie)
        //                .Select(m => _movieConverter.ConvertDt(m));
        //    var result = Pagination.GetPagedData(query, pageSize, pageNumber);
        //    return result;
        //}
        // Sắp xếp theo ngày công chiếu mới nhất
        public async Task<PageResult<DataResponsesMovie>> GetFeaturedMovies(int pageSize, int pageNumber)
        {
            var query =  _appDbContext.Movies
                        .AsNoTracking()
                        .Where(m => m.IsActive == true)
                        .OrderByDescending(m => m.PremiereDate) 
                        .Select(m => _movieConverter.ConvertDt(m));

            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;
        }

    }
}
