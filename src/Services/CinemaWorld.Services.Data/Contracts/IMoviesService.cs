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

        Task<IEnumerable<TEntity>> GetAllMoviesAsync<TEntity>();

        Task<IEnumerable<TEntity>> GetTopMoviesAsync<TEntity>(decimal rating = 0);

        Task<IEnumerable<TEntity>> GetAllMovieGenresAsync<TEntity>();

        Task<IEnumerable<TEntity>> GetAllMovieCountriesAsync<TEntity>();

        IQueryable<TViewModel> GetAllMoviesAsQueryeable<TViewModel>();

        Task<IEnumerable<MovieDetailsViewModel>> GetByGenreNameAsync(string name);
    }
}
