namespace CinemaWorld.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CinemaWorld.Models.InputModels.AdministratorInputModels.MovieProjections;
    using CinemaWorld.Models.ViewModels.MovieProjections;

    public interface IMovieProjectionsService : IBaseDataService
    {
        Task<MovieProjectionDetailsViewModel> CreateAsync(MovieProjectionCreateInputModel model);

        Task EditAsync(MovieProjectionEditViewModel movieProjectionEditViewModel);

        Task<IEnumerable<TViewModel>> GetAllMovieProjectionsAsync<TViewModel>();

        IQueryable<TViewModel> GetAllMovieProjectionsByCinemaAsQueryeable<TViewModel>(string cinemaName = null);

        IQueryable<TViewModel> GetAllMovieProjectionsAsQueryeable<TViewModel>();
    }
}
