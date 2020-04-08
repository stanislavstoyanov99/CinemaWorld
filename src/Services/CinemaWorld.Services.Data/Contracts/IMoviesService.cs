namespace CinemaWorld.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CinemaWorld.Models.InputModels.AdministratorInputModels.Movies;
    using CinemaWorld.Models.ViewModels.Movies;

    public interface IMoviesService : IBaseDataService
    {
        Task<MovieDetailsViewModel> CreateAsync(MovieCreateInputModel movieCreateInputModel);

        Task EditAsync(MovieEditViewModel movieEditViewModel);

        Task<IEnumerable<TViewModel>> GetAllMoviesAsync<TViewModel>();

        Task<IEnumerable<TViewModel>> GetTopMoviesAsync<TViewModel>(decimal rating = 0);

        Task<IEnumerable<TViewModel>> GetRecentlyAddedMoviesAsync<TViewModel>(int count = 0);

        Task<IEnumerable<TViewModel>> GetAllMovieGenresAsync<TViewModel>();

        Task<IEnumerable<TViewModel>> GetAllMovieCountriesAsync<TViewModel>();

        IQueryable<TViewModel> GetAllMoviesAsQueryeable<TViewModel>();

        Task<IEnumerable<MovieDetailsViewModel>> GetByGenreNameAsync(string name);
    }
}
