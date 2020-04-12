namespace CinemaWorld.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CinemaWorld.Models.InputModels.AdministratorInputModels.Seats;
    using CinemaWorld.Models.ViewModels.Seats;

    public interface ISeatsService : IBaseDataService
    {
        Task<SeatDetailsViewModel> CreateAsync(SeatCreateInputModel seatCreateInputModel);

        Task EditAsync(SeatEditViewModel seatEditViewModel);

        Task<IEnumerable<TViewModel>> GetAllSeatsAsync<TViewModel>();

        IQueryable<TViewModel> GetAllSeatsAsQueryeable<TViewModel>();
    }
}
