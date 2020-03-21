namespace CinemaWorld.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CinemaWorld.Models.InputModels.AdministratorInputModels.Countries;
    using CinemaWorld.Models.ViewModels.Countries;

    public interface ICountriesService : IBaseDataService
    {
        Task<CountryDetailsViewModel> CreateAsync(CountryCreateInputModel countryCreateInputModel);

        Task EditAsync(CountryEditViewModel countryEditViewModel);

        Task<IEnumerable<TEntity>> GetAllCountriesAsync<TEntity>();
    }
}
