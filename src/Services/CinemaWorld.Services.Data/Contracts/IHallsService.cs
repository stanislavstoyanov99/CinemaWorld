namespace CinemaWorld.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CinemaWorld.Models.InputModels.AdministratorInputModels.Halls;
    using CinemaWorld.Models.ViewModels.Halls;

    public interface IHallsService : IBaseDataService
    {
        Task<HallDetailsViewModel> CreateAsync(HallCreateInputModel hallCreateInputModel);

        Task EditAsync(HallEditViewModel hallEditViewModel);

        Task<IEnumerable<TViewModel>> GetAllHallsAsync<TViewModel>();
    }
}
