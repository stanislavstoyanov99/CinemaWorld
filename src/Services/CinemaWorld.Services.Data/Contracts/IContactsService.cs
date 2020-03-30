namespace CinemaWorld.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CinemaWorld.Models.InputModels.AdministratorInputModels.Contacts;
    using CinemaWorld.Models.ViewModels.Contacts;

    public interface IContactsService
    {
        Task SendContactToAdmin(ContactFormEntryViewModel contactFormEntryViewModel);

        Task SendContactToUser(SendContactInputModel sendContactInputModel);

        Task<IEnumerable<TEntity>> GetAllUserEmailsAsync<TEntity>();
    }
}
