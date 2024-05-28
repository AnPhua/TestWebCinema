using Microsoft.EntityFrameworkCore;
using WebXemPhim.Entities;
using WebXemPhim.Handle.HandlePagination;
using WebXemPhim.Payloads.Converters;
using WebXemPhim.Payloads.DataRequests;
using WebXemPhim.Payloads.DataResponses;
using WebXemPhim.Payloads.Responses;
using WebXemPhim.Services.Interfaces;

namespace WebXemPhim.Services.Implements
{
    public class ScheduleServices:BaseServices,IScheduleServices
    {
        private readonly ResponseObject<DataResponsesSchedule> _responseObject;
        private readonly SchedulesConverter _converter;
        public ScheduleServices(SchedulesConverter converter, ResponseObject<DataResponsesSchedule> _responseObject)
        {
            _converter = converter;
            this._responseObject = _responseObject;
        }

        public async Task<ResponseObject<DataResponsesSchedule>> CreateSchedule(Requests_CreateSchedule request)
        {
            var room = await _appDbContext.Rooms.SingleOrDefaultAsync(x => x.Id == request.RoomId);
            if (room == null)
            {
                return _responseObject.ResponseFail(StatusCodes.Status404NotFound, "Không Tìm Thấy Phòng", null);
            }

            var movie = await _appDbContext.Movies.SingleOrDefaultAsync(x => x.Id == request.MovieId);
            if (movie == null)
            {
                return _responseObject.ResponseFail(StatusCodes.Status404NotFound, "Không Tìm Thấy Phim", null);
            }

            var endAt = request.StartAt.AddMinutes(movie.MovieDuration + 30);


            if (_appDbContext.Schedules
                .Any(x => !((request.StartAt < x.StartAt && endAt < x.StartAt) || (request.StartAt > x.EndAt && endAt > x.EndAt)) && x.RoomId == request.RoomId && x.IsActive == true))
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Lịch Chiếu Bị Trùng", null);
            }

            var schedule = new Schedule
            {
                RoomId = room.Id,
                MovieId = movie.Id,
                Code = DateTime.Now.Ticks.ToString() + "schmov" + new Random().Next(1000, 9999),
                StartAt = request.StartAt,
                EndAt = endAt, 
                Name = "Lịch Chiếu : " + movie.Name,
            };

            await _appDbContext.Schedules.AddAsync(schedule);
            await _appDbContext.SaveChangesAsync();

            return _responseObject.ResponseSucess("Thêm Suất Chiếu Thành Công", _converter.ConvertDt(schedule));
        }


        public async Task<ResponseObject<DataResponsesSchedule>> UpdateSchedule(Requests_UpdateSchedule request)
        {
            var schedule = await _appDbContext.Schedules.SingleOrDefaultAsync(x => x.Id == request.ScheduleId);
            if (schedule == null)
            {
                return _responseObject.ResponseFail(StatusCodes.Status404NotFound, "Suất Chiếu Không Tồn Tại", null);
            }

            if (!_appDbContext.Rooms.Any(x => x.Id == request.RoomId))
            {
                return _responseObject.ResponseFail(StatusCodes.Status404NotFound, "Không Tìm Thấy Phòng", null);
            }
            var movie = await _appDbContext.Movies.SingleOrDefaultAsync(x => x.Id == request.MovieId);
            if (movie == null)
            {
                return _responseObject.ResponseFail(StatusCodes.Status404NotFound, "Không Tìm Thấy Phim", null);
            }

            var newDuration = await _appDbContext.Movies
                .Where(x => x.Id == request.MovieId)
                .Select(x => x.MovieDuration)
                .FirstOrDefaultAsync();

            var newEndAt = request.StartAt.AddMinutes(newDuration + 30);

            

            if (_appDbContext.Schedules
                .Any(x => !((request.StartAt < x.StartAt && newEndAt < x.StartAt) || (request.StartAt > x.EndAt && newEndAt > x.EndAt)) && x.RoomId == request.RoomId && x.IsActive == true))
            {
                return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Lịch Chiếu Bị Trùng", null);
            }

            schedule.StartAt = request.StartAt;
            schedule.Name = "Lịch Chiếu : " + movie.Name;
            schedule.Code = DateTime.Now.Ticks.ToString() + "schmov" + new Random().Next(100, 999);
            schedule.MovieId = request.MovieId;
            schedule.RoomId = request.RoomId;
            schedule.EndAt = newEndAt;

            _appDbContext.Schedules.Update(schedule);
            await _appDbContext.SaveChangesAsync();
            return _responseObject.ResponseSucess("Cập Nhật Thông Tin Suất Chiếu Thành Công", _converter.ConvertDt(schedule));
        }



        public async Task<PageResult<DataResponsesSchedule>> GetSchedulesByMovie(int movieId, int pageSize, int pageNumber)
        {
            var query = _appDbContext.Schedules.Where(x => x.MovieId == movieId).Select(x => _converter.ConvertDtforticket(x));
            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;
        }
        public async Task<PageResult<DataResponsesScheduleForDate>> GetSchedulesMovielistHours(int movieId, int pageSize, int pageNumber)
        {
            var schedules = await _appDbContext.Schedules
                .Where(x => x.MovieId == movieId)
                .ToListAsync();

            var dataResponses = _converter.ConvertDatafordaySort(schedules);

            var pagedData = Pagination.GetPagedData(dataResponses.AsQueryable(), pageSize, pageNumber);

            return pagedData;
        }
        public async Task<PageResult<DataResponsesScheduleForDate>> GetSchedulesDaylistHour(InputDtScheduleDay dt, int pageSize, int pageNumber)
        {
            var targetDate = dt.TheDay.Value.Date;
            var schedules = await _appDbContext.Schedules
            .Where(x => x.StartAt.Year == targetDate.Year &&
                        x.StartAt.Month == targetDate.Month &&
                        x.StartAt.Day == targetDate.Day &&x.IsActive == true)
            .ToListAsync();

            var dataResponses = _converter.ConvertDataforday(schedules);

            var pagedData = Pagination.GetPagedData(dataResponses.AsQueryable(), pageSize, pageNumber);

            return pagedData;
        }
        public async Task<PageResult<DataResponsesGetDays>> GetSchedulesForAllDays(int pageSize, int pageNumber)
        {
            var today = DateTime.Today;
            var schedules = await _appDbContext.Schedules
            .Where(x => x.IsActive == true && x.StartAt >= today)
            .ToListAsync();

            var dataResponses = _converter.ConverForGetDays(schedules);

            var pagedData = Pagination.GetPagedData(dataResponses.AsQueryable(), pageSize, pageNumber);

            return pagedData;
        }


        public async Task<PageResult<DataResponsesSchedule>> GetAlls(InputScheduleData input, int pageSize, int pageNumber)
        {
            var query = await _appDbContext.Schedules.Where(x => x.IsActive == true).Include(x => x.Room).ToListAsync();
            if (input.RoomId.HasValue)
            {
                query = query.Where(x => x.RoomId == input.RoomId).ToList();
            }
            var result = Pagination.GetPagedData(query.Select(x => _converter.ConvertDt(x)).AsQueryable(), pageSize, pageNumber);
            return result;
        }
        public IEnumerable<Schedule> GetAllSchedulesNoPagination()
        {
            return _appDbContext.Schedules.Where(x => x.IsActive == true && x.StartAt > DateTime.Now).AsQueryable();
        }
        public async Task<ResponseObject<DataResponsesSchedule>> DeleteSchedule(int scheduleId)
        {
            var schedule = await _appDbContext.Schedules.SingleOrDefaultAsync(x => x.Id == scheduleId);
            if (schedule.EndAt < DateTime.Now && schedule != null)
            {
                schedule.IsActive = false;
                _appDbContext.Schedules.Update(schedule);
                await _appDbContext.SaveChangesAsync();
                return _responseObject.ResponseSucess("Xóa Suất Chiếu Thành Công!", _converter.ConvertDt(schedule));
            }
            return _responseObject.ResponseFail(StatusCodes.Status400BadRequest, "Chưa Hết Giờ Chiếu ,Không Thể Xóa!", null);

        }

        public async Task<PageResult<DataResponsesSchedule>> GetSchedulesByDay(DateTime startAt, int pageSize, int pageNumber)
        {
            var query = _appDbContext.Schedules
                .Include(x => x.Movie)
                .AsNoTracking()
                .Where(x => x.StartAt.Date == startAt.Date)
                .Select(x => _converter.ConvertDt(x));

            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;
        }

        public async Task<ResponseObject<DataResponsesSchedule>> GetSchedulesById(int schId)
        {
            var result = await _appDbContext.Schedules.SingleOrDefaultAsync(x => x.Id == schId);
            if (result == null)
            {
                return _responseObject.ResponseFail(StatusCodes.Status404NotFound, "Không Tìm Thấy Id Lịch Chiếu", null);
            }
            return _responseObject.ResponseSucess("Lấy Dữ Liệu Thành Công", _converter.ConvertDtforticket(result));
        }
    }
}

