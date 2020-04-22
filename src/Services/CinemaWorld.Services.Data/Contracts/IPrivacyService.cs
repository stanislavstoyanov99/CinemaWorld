namespace CinemaWorld.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using CinemaWorld.Models.InputModels.AdministratorInputModels.Privacy;
    using CinemaWorld.Models.ViewModels.Privacy;

    public interface IPrivacyService : IBaseDataService
    {
        Task<PrivacyDetailsViewModel> CreateAsync(PrivacyCreateInputModel privacyCreateInputModel);

        Task EditAsync(PrivacyEditViewModel privacyEditViewModel);

        Task<TViewModel> GetViewModelAsync<TViewModel>();
    }
}
