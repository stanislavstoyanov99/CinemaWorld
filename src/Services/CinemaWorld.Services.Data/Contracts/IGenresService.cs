namespace CinemaWorld.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CinemaWorld.Models.InputModels.AdministratorInputModels.Genres;
    using CinemaWorld.Models.ViewModels.Genres;

    public interface IGenresService : IBaseDataService
    {
        Task<GenreDetailsViewModel> CreateAsync(GenreCreateInputModel genreCreateInputModel);

        Task EditAsync(GenreEditViewModel genreEditViewModel);

        Task<IEnumerable<TEntity>> GetAllGenresAsync<TEntity>();
    }
}
