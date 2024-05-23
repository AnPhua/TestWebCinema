using WebXemPhim.Payloads.DataResponses;
using WebXemPhim.Payloads.Responses;
using WebXemPhim.Payloads.DataRequests;
using WebXemPhim.Handle.HandlePagination;
using Microsoft.AspNetCore.Components.Forms;
using WebXemPhim.Entities;

namespace WebXemPhim.Services.Interfaces
{
    public interface IMovieServices
    {
        Task<ResponseObject<DataResponsesMovie>> CreateMovie(Requests_CreateMovie requests);
        Task<ResponseObject<DataResponsesMovie>> UpdateMovie(Requests_UpdateMovie requests);
        Task<ResponseObject<DataResponsesMovie>> UpdateMovieHaveString(Requests_UpdateMovieHaveString requests);
        Task<ResponseObject<DataResponsesMovie>> UpdateMovieImageString(Requests_UpdateMovieImageString requests);
        Task<ResponseObject<DataResponsesMovie>> UpdateMovieHeroImageString(Requests_UpdateMovieHeroImageString requests);
        string DeleteMovie(int movieId);
        ResponseObject<DataResponsesRate> CreateRate(Requests_CreateRate requests);
        ResponseObject<DataResponsesRate> UpdateRate(Requests_UpdateRate requests);
        string DeleteRate(int rateId);
        ResponseObject<DataResponsesMovieType> CreateMovieType(Requests_CreateMovieType requests);
        ResponseObject<DataResponsesMovieType> UpdateMovieType(Requests_UpdateMovieType requests);
        string DeleteMovieType(int movieTypeId);
        Task<PageResult<DataResponsesMovie>> GetFeaturedMovies(int pageSize, int pageNumber);
        Task<PageResult<DataResponsesMovie>> GetMovieUnreference(InputDt dt,int pageSize, int pageNumber);
        Task<PageResult<DataResponsesMovie>> GetMovieShowing(InputDt dt, int pageSize, int pageNumber);
        Task<PageResult<DataResponsesMovieType>> GetAllMovieTypes(int pageSize, int pageNumber);
        IEnumerable<MovieType> GetAllMovieTypeNoPagination();
        IEnumerable<Rate> GetAllRateTypeNoPagination();
        IEnumerable<Movie> GetAllMovieNoPagination();
        Task<ResponseObject<DataResponsesMovieType>> GetMovieTypeById(int movieTypeId);
        Task<PageResult<DataResponsesMovie>> GetAllMovie(InputFilter input, int pageSize, int pageNumber);
        Task<PageResult<DataResponsesMovie>> GetAllMovieSpecial(InputFilter input, int pageSize, int pageNumber);
        Task<ResponseObject<DataResponsesMovie>> GetMovieById(int movieId);
        Task<ResponseObject<DataResponsesMovie>> GetMovieByIdForSort(int movieId);
    }
}
