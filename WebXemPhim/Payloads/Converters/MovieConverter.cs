﻿using WebXemPhim.Entities;
using WebXemPhim.Payloads.DataResponses;

namespace WebXemPhim.Payloads.Converters
{
    public class MovieConverter
    {
        private readonly AppDbContext _appDbContext;
        private readonly SchedulesConverter _converter;
        public MovieConverter()
        {
            _appDbContext = new AppDbContext();
            _converter = new SchedulesConverter();
        }
        public DataResponsesMovie ConvertDt(Movie movie)
        {
            return new DataResponsesMovie
            {
                Description = movie.Description,
                Director = movie.Director,
                Caster = movie.Caster,
                IsHot = movie.IsHot,
                EndTime = movie.EndTime,
                PremiereDate = movie.PremiereDate,
                Id = movie.Id,
                Image = movie.Image,
                HeroImage = movie.HeroImage,
                Language = movie.Language,
                MovieDuration = movie.MovieDuration,
                IsSellTicket = movie.IsSellTicket,
                MovieTypeName = _appDbContext.MovieTypes.SingleOrDefault(x => x.Id == movie.MovieTypeId).MovieTypeName,
                Name = movie.Name,
                RateName = _appDbContext.Rates.SingleOrDefault(x=>x.Id == movie.RateId).Code +" - "+ _appDbContext.Rates.SingleOrDefault(x => x.Id == movie.RateId).Description,
                Trailer = movie.Trailer,
                Schedules = _appDbContext.Schedules.Where(x => x.MovieId == movie.Id).Select(x => _converter.ConvertDt(x))
            };
        }
        public DataResponsesMovie ConvertDtID(Movie movie)
        {
            return new DataResponsesMovie
            {
                Description = movie.Description,
                Director = movie.Director,
                Caster = movie.Caster,
                IsHot = movie.IsHot,
                EndTime = movie.EndTime,
                PremiereDate = movie.PremiereDate,
                Id = movie.Id,
                Image = movie.Image,
                HeroImage = movie.HeroImage,
                Language = movie.Language,
                MovieDuration = movie.MovieDuration,
                IsSellTicket = movie.IsSellTicket,
                MovieTypeName = movie.MovieTypeId.ToString(),
                Name = movie.Name,
                RateName = movie.RateId.ToString(),
                Trailer = movie.Trailer,
                Schedules = _appDbContext.Schedules.Where(x => x.MovieId == movie.Id).Select(x => _converter.ConvertDt(x))
            };
        }
    }
}
