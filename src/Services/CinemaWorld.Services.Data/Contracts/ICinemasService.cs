namespace CinemaWorld.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CinemaWorld.Models.InputModels.AdministratorInputModels.Cinemas;
    using CinemaWorld.Models.ViewModels.Cinemas;

    public interface ICinemasService : IBaseDataService
    {
        Task<CinemaDetailsViewModel> CreateAsync(CinemaCreateInputModel cinemaCreateInputModel);

        Task EditAsync(CinemaEditViewModel cinemaEditViewModel);

        Task<IEnumerable<TEntity>> GetAllCinemasAsync<TEntity>();
    }
}
