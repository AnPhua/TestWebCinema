﻿using Microsoft.EntityFrameworkCore;
using System;
using WebXemPhim.Entities;
using WebXemPhim.Payloads.DataResponses;

namespace WebXemPhim.Payloads.Converters
{
    public class SchedulesConverter
    {
        private readonly AppDbContext _appDbContext;
        private readonly TicketConverter _ticketConverter;
        public SchedulesConverter()
        {
            _appDbContext = new AppDbContext();
            _ticketConverter = new TicketConverter();
        }
        public DataResponsesSchedule ConvertDt(Schedule schedule)
        {
            var tickets = _appDbContext.Tickets
            .Where(x => x.ScheduleId == schedule.Id)
            .ToList();

            var emptySeats = tickets.Count(x => x.TypeTicket == 1);

          
            return new DataResponsesSchedule
            {
                Id = schedule.Id,
                MovieName = _appDbContext.Movies.SingleOrDefault(x => x.Id == schedule.MovieId).Name,
                StartAt = schedule.StartAt,
                EndAt = schedule.EndAt,
                Code = schedule.Code,
                Name = schedule.Name,
                EmptySeat = emptySeats,
                RoomName = _appDbContext.Rooms.SingleOrDefault(x => x.Id == schedule.RoomId).Name,
            };
           
        }
        public DataResponsesSchedule ConvertDtSort(Schedule schedule)
        {
            if (schedule.StartAt < DateTime.Today)
            {
                return null; 
            }
            var tickets = _appDbContext.Tickets
            .Where(x => x.ScheduleId == schedule.Id)
            .ToList();

            var emptySeats = tickets.Count(x => x.TypeTicket == 1);


            return new DataResponsesSchedule
            {
                Id = schedule.Id,
                MovieName = _appDbContext.Movies.SingleOrDefault(x => x.Id == schedule.MovieId).Name,
                StartAt = schedule.StartAt,
                EndAt = schedule.EndAt,
                Code = schedule.Code,
                Name = schedule.Name,
                EmptySeat = emptySeats,
                RoomName = _appDbContext.Rooms.SingleOrDefault(x => x.Id == schedule.RoomId).Name,
            };

        }
        public DataResponsesSchedule ConvertDtforticket(Schedule schedule)
        {
            var tickets = _appDbContext.Tickets
            .Where(x => x.ScheduleId == schedule.Id)
            .ToList();

            var emptySeats = tickets.Count(x => x.TypeTicket == 1);
            return new DataResponsesSchedule
            {
                Id = schedule.Id,
                MovieName = _appDbContext.Movies.SingleOrDefault(x => x.Id == schedule.MovieId).Name,
                StartAt = schedule.StartAt,
                EndAt = schedule.EndAt,
                Code = schedule.Code,
                Name = schedule.Name,
                RoomName = _appDbContext.Rooms.SingleOrDefault(x => x.Id == schedule.RoomId).Name,
                EmptySeat = emptySeats,
                DataResponsesTicketforsche = _appDbContext.Tickets
                .Where(x => x.ScheduleId == schedule.Id)
                .OrderBy(x => _appDbContext.Seats.SingleOrDefault(s => s.Id == x.SeatId).Line)
                .ThenBy(x => _appDbContext.Seats.SingleOrDefault(s => s.Id == x.SeatId).Number)
                .Select(x => _ticketConverter.ConvertDtandticket(x))
                .AsQueryable()
            };
        }
        private string GetShortDayOfWeek(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    return "T2";
                case DayOfWeek.Tuesday:
                    return "T3";
                case DayOfWeek.Wednesday:
                    return "T4";
                case DayOfWeek.Thursday:
                    return "T5";
                case DayOfWeek.Friday:
                    return "T6";
                case DayOfWeek.Saturday:
                    return "T7";
                case DayOfWeek.Sunday:
                    return "CN";
                default:
                    return "";
            }
        }
        public List<DataResponsesScheduleForDate> ConvertDataforday(IEnumerable<Schedule> schedules)
        {
            var groupedSchedules = schedules
                .GroupBy(s => new
                {
                    s.MovieId,s.StartAt.Day
                });

            var results = new List<DataResponsesScheduleForDate>();

            foreach (var group in groupedSchedules)
            {
                var tickets = _appDbContext.Tickets
                    .Where(x => group.Select(g => g.Id).Contains(x.ScheduleId))
                    .ToList();

                var listTimeinSchedules = group.Select(g =>
                {
                    var scheduleTickets = tickets.Where(x => x.ScheduleId == g.Id).ToList();
                    var emptySeats = scheduleTickets.Count(x => x.TypeTicket == 1);
                    return new ControlDate
                    {
                        Id = g.Id,
                        TimeDt = g.StartAt.ToString("HH:mm"),
                        StartAt = g.StartAt,
                        EmptySeat = emptySeats
                    };
                }).ToList();

                var firstSchedule = group.First();

                DateTime dt = firstSchedule.StartAt;
                DayOfWeek dayOfWeek = dt.DayOfWeek;
                string shortDayOfWeek = GetShortDayOfWeek(dayOfWeek);

                results.Add(new DataResponsesScheduleForDate
                {
                    Id = firstSchedule.Id,
                    MovieId = firstSchedule.MovieId,
                    RoomName = _appDbContext.Rooms.SingleOrDefault(x => x.Id == firstSchedule.RoomId).Name,
                    Day = dt.Day,
                    Month = dt.Month,
                    Year = dt.Year,
                    DayDetails = dt.Day + "/" + dt.Month + "/" + dt.Year,
                    Date = shortDayOfWeek,
                    ListTimeinSchedules = listTimeinSchedules.AsQueryable()
                });
            }

            return results;
        }
        //public List<DataResponsesScheduleForDatePlus> ConvertDatafordaySort(IEnumerable<Schedule> schedules)
        //{
        //    var filteredSchedules = schedules
        //       .Where(s => s.StartAt >= DateTime.Now)
        //       .OrderBy(s => s.StartAt);
        //    var groupedSchedules = filteredSchedules
        //        .GroupBy(s => new
        //        {
        //            s.MovieId,
        //            s.RoomId,
        //            Day = s.StartAt.Day,
        //            Month = s.StartAt.Month,
        //            Year = s.StartAt.Year
        //        });

        //    var results = new List<DataResponsesScheduleForDatePlus>();

        //    foreach (var group in groupedSchedules)
        //    {
        //        var tickets = _appDbContext.Tickets
        //            .Where(x => group.Select(g => g.Id).Contains(x.ScheduleId))
        //            .ToList();

        //        var listTimeinSchedules = group.Select(g =>
        //        {
        //            var scheduleTickets = tickets.Where(x => x.ScheduleId == g.Id).ToList();
        //            var emptySeats = scheduleTickets.Count(x => x.TypeTicket == 1 && x.IsActive == true);
        //            var roomName = _appDbContext.Rooms.SingleOrDefault(r => r.Id == g.RoomId)?.Name;
        //            return new ControlDatePlus
        //            {
        //                Id = g.Id,
        //                TimeDt = g.StartAt.ToString("HH:mm"),
        //                RoomName = roomName,
        //                StartAt = g.StartAt,
        //                EmptySeat = emptySeats
        //            };
        //        }).ToList();

        //        var firstSchedule = group.First();

        //        DateTime dt = firstSchedule.StartAt;
        //        DayOfWeek dayOfWeek = dt.DayOfWeek;
        //        string shortDayOfWeek = GetShortDayOfWeek(dayOfWeek);

        //        results.Add(new DataResponsesScheduleForDatePlus
        //        {
        //            Id = firstSchedule.Id,
        //            MovieId = firstSchedule.MovieId,
        //            Day = dt.Day,
        //            Month = dt.Month,
        //            Year = dt.Year,
        //            DayDetails = dt.Day + "/" + dt.Month + "/" + dt.Year,
        //            Date = shortDayOfWeek,
        //            ListTimeinSchedules = listTimeinSchedules.AsQueryable()
        //        });
        //    }

        //    return results;
        //}
        public List<DataResponsesScheduleForDatePlus> ConvertDatafordaySort(IEnumerable<Schedule> schedules)
        {
            var filteredSchedules = schedules
                .Where(s => s.StartAt >= DateTime.Now);

            var groupedSchedules = filteredSchedules
                .GroupBy(s => new
                {
                    s.MovieId,s.StartAt.Day
                });

            var results = new List<DataResponsesScheduleForDatePlus>();

            foreach (var group in groupedSchedules)
            {
                var tickets = _appDbContext.Tickets
                    .Where(x => group.Select(g => g.Id).Contains(x.ScheduleId))
                    .ToList();

                var listTimeinSchedules = new List<ControlDatePlus>();

                foreach (var schedule in group)
                {
                    var scheduleTickets = tickets.Where(x => x.ScheduleId == schedule.Id).ToList();
                    var emptySeats = scheduleTickets.Count(x => x.TypeTicket == 1 && x.IsActive == true);
                    var roomName = _appDbContext.Rooms.SingleOrDefault(r => r.Id == schedule.RoomId)?.Name;

                    listTimeinSchedules.Add(new ControlDatePlus
                    {
                        Id = schedule.Id,
                        TimeDt = schedule.StartAt.ToString("HH:mm"),
                        RoomName = roomName,
                        StartAt = schedule.StartAt,
                        EmptySeat = emptySeats
                    });
                }

                var firstSchedule = group.First();

                DateTime dt = firstSchedule.StartAt;
                DayOfWeek dayOfWeek = dt.DayOfWeek;
                string shortDayOfWeek = GetShortDayOfWeek(dayOfWeek);

                results.Add(new DataResponsesScheduleForDatePlus
                {
                    Id = firstSchedule.Id,
                    MovieId = firstSchedule.MovieId,
                    Day = dt.Day,
                    Month = dt.Month,
                    Year = dt.Year,
                    DayDetails = $"{dt.Day}/{dt.Month}/{dt.Year}",
                    Date = shortDayOfWeek,
                    ListTimeinSchedules = listTimeinSchedules.AsQueryable()
                });
            }

            return results;
        }
        public List<DataResponsesMovieDetailsSchedule> ConvertDatafordaySortaddmv(IEnumerable<Schedule> schedules)
        {
            var filteredSchedules = schedules
                .Where(s => s.StartAt >= DateTime.Now);

            var groupedSchedules = filteredSchedules
                .GroupBy(s => new
                {
                    s.MovieId,
                    s.StartAt.Day
                });

            var results = new List<DataResponsesMovieDetailsSchedule>();

            foreach (var group in groupedSchedules)
            {
                var tickets = _appDbContext.Tickets
                    .Where(x => group.Select(g => g.Id).Contains(x.ScheduleId))
                    .ToList();

                var listTimeinSchedules = new List<ControlDatePlus>();

                foreach (var schedule in group)
                {
                    var scheduleTickets = tickets.Where(x => x.ScheduleId == schedule.Id).ToList();
                    var emptySeats = scheduleTickets.Count(x => x.TypeTicket == 1 && x.IsActive == true);
                    var roomName = _appDbContext.Rooms.SingleOrDefault(r => r.Id == schedule.RoomId)?.Name;

                    listTimeinSchedules.Add(new ControlDatePlus
                    {
                        Id = schedule.Id,
                        TimeDt = schedule.StartAt.ToString("HH:mm"),
                        RoomName = roomName,
                        StartAt = schedule.StartAt,
                        EmptySeat = emptySeats
                    });
                }

                var firstSchedule = group.FirstOrDefault();
                //if (firstSchedule == null)
                //{
                //    continue;
                //}

                var movie = _appDbContext.Movies.SingleOrDefault(x => x.Id == firstSchedule.MovieId);
                //if (movie == null)
                //{
                //    continue;
                //}
                var movieTypeName = _appDbContext.MovieTypes.SingleOrDefault(mt => mt.Id == movie.MovieTypeId)?.MovieTypeName;
                var rateName = _appDbContext.Rates.SingleOrDefault(mt => mt.Id == movie.RateId)?.Code;
                DateTime dt = firstSchedule.StartAt;
                DayOfWeek dayOfWeek = dt.DayOfWeek;
                string shortDayOfWeek = GetShortDayOfWeek(dayOfWeek);
                results.Add(new DataResponsesMovieDetailsSchedule
                {
                    Id = firstSchedule.Id,
                    Day = dt.Day,
                    Month = dt.Month,
                    Year = dt.Year,
                    DayDetails = $"{dt.Day}/{dt.Month}/{dt.Year}",
                    Date = shortDayOfWeek,
                    MovieId = movie.Id,
                    Image = movie.Image,
                    MovieTypeName = movieTypeName,
                    Name = movie.Name,
                    RateCode = rateName,
                    Trailer = movie.Trailer,
                    MovieDuration = movie.MovieDuration,
                    ListTimeinSchedules = listTimeinSchedules.AsQueryable()
                });
            }

            return results;
        }
        public List<DataResponsesGetDays> ConverForGetDays(IEnumerable<Schedule> schedules)
        {
            var groupedSchedules = schedules
                .GroupBy(s => new
                {
                    s.StartAt.Date
                });

            var results = new List<DataResponsesGetDays>();

            foreach (var group in groupedSchedules)
            {
                var firstSchedule = group.First();

                DateTime dt = firstSchedule.StartAt;
                DayOfWeek dayOfWeek = dt.DayOfWeek;
                string shortDayOfWeek = GetShortDayOfWeek(dayOfWeek);

                results.Add(new DataResponsesGetDays
                {
                    Id = firstSchedule.Id,
                    Day = dt.Day,
                    Month = dt.Month,
                    Year = dt.Year,
                    DayDetails = dt.ToString("yyyy-MM-dd"),
                    Date = shortDayOfWeek
                });
            }

            return results.OrderBy(r => r.Year)
                          .ThenBy(r => r.Month)
                          .ThenBy(r => r.Day)
                          .ToList();
        }
    }
}
