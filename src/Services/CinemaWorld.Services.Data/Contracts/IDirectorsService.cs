namespace CinemaWorld.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using CinemaWorld.Models.InputModels.AdministratorInputModels.Directors;
    using CinemaWorld.Models.ViewModels.Directors;

    public interface IDirectorsService : IBaseDataService
    {
        Task<DirectorViewModel> CreateAsync(DirectorCreateInputModel directorCreateInputModel);

        // Task EditAsync(DirectorEditViewModel directorEditViewModel);

        Task<IEnumerable<DirectorViewModel>> GetAllDirectorsAsync();

        IEnumerable<SelectListItem> SelectListDirectors();
    }
}
