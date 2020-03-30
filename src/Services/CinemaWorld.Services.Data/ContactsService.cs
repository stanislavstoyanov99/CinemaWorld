namespace CinemaWorld.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CinemaWorld.Common;
    using CinemaWorld.Data.Common.Repositories;
    using CinemaWorld.Data.Models;
    using CinemaWorld.Models.InputModels.AdministratorInputModels.Contacts;
    using CinemaWorld.Models.ViewModels.Contacts;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Services.Mapping;
    using CinemaWorld.Services.Messaging;

    using Microsoft.EntityFrameworkCore;

    public class ContactsService : IContactsService
    {
        private readonly IRepository<ContactFormEntry> userContactsRepository;
        private readonly IRepository<AdminContactFromEntry> adminContactsRepository;
        private readonly IEmailSender emailSender;

        public ContactsService(
            IRepository<ContactFormEntry> contactsRepository,
            IRepository<AdminContactFromEntry> adminContactsRepository,
            IEmailSender emailSender)
        {
            this.userContactsRepository = contactsRepository;
            this.adminContactsRepository = adminContactsRepository;
            this.emailSender = emailSender;
        }

        public async Task<IEnumerable<TModel>> GetAllUserEmailsAsync<TModel>()
        {
            var userEmails = await this.userContactsRepository
                .All()
                .To<TModel>()
                .ToListAsync();

            return userEmails;
        }

        public async Task SendContactToAdmin(ContactFormEntryViewModel contactFormViewModel)
        {
            var contactFormEntry = new ContactFormEntry
            {
                FirstName = contactFormViewModel.FirstName,
                LastName = contactFormViewModel.LastName,
                Email = contactFormViewModel.Email,
                Subject = contactFormViewModel.Subject,
                Content = contactFormViewModel.Content,
            };

            await this.userContactsRepository.AddAsync(contactFormEntry);
            await this.userContactsRepository.SaveChangesAsync();

            await this.emailSender.SendEmailAsync(
                contactFormViewModel.Email,
                string.Concat(contactFormViewModel.FirstName, " ", contactFormViewModel.LastName),
                GlobalConstants.SystemEmail,
                contactFormViewModel.Subject,
                contactFormViewModel.Content);
        }

        public async Task SendContactToUser(SendContactInputModel sendContactInputModel)
        {
            var adminContactFormEntry = new AdminContactFromEntry
            {
                FullName = sendContactInputModel.FullName,
                Email = sendContactInputModel.Email,
                Subject = sendContactInputModel.Subject,
                Content = sendContactInputModel.Content,
            };

            await this.adminContactsRepository.AddAsync(adminContactFormEntry);
            await this.adminContactsRepository.SaveChangesAsync();

            await this.emailSender.SendEmailAsync(
                GlobalConstants.SystemEmail,
                sendContactInputModel.FullName,
                sendContactInputModel.Email,
                sendContactInputModel.Subject,
                sendContactInputModel.Content);
        }
    }
}
