namespace CinemaWorld.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CinemaWorld.Models.ViewModels.Tickets;

    public interface ITicketsService : IBaseDataService
    {
        Task<TicketDetailsViewModel> BuyAsync(TicketInputModel ticket, int movieProjectionId);
    }
}
