namespace CinemaWorld.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CinemaWorld.Models.InputModels.AdministratorInputModels.Directors;
    using CinemaWorld.Models.ViewModels.Directors;

    public interface IDirectorsService : IBaseDataService
    {
        Task<DirectorDetailsViewModel> CreateAsync(DirectorCreateInputModel directorCreateInputModel);

        Task EditAsync(DirectorEditViewModel directorEditViewModel);

        Task<IEnumerable<TViewModel>> GetAllDirectorsAsync<TViewModel>();
    }
}
