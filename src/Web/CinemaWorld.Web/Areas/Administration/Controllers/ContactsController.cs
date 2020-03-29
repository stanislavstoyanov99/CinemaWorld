namespace CinemaWorld.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CinemaWorld.Common;
    using CinemaWorld.Data.Common.Repositories;
    using CinemaWorld.Data.Models;
    using CinemaWorld.Models.InputModels.AdministratorInputModels.Contacts;
    using CinemaWorld.Services.Messaging;

    using Microsoft.AspNetCore.Mvc;

    public class ContactsController : AdministrationController
    {
        private readonly IRepository<AdminContactFromEntry> adminContactsRepository;
        private readonly IRepository<ContactFormEntry> contactFormRepository;
        private readonly IEmailSender emailSender;

        public ContactsController(
            IRepository<AdminContactFromEntry> adminContactsRepository,
            IRepository<ContactFormEntry> contactFormRepository,
            IEmailSender emailSender)
        {
            this.adminContactsRepository = adminContactsRepository;
            this.contactFormRepository = contactFormRepository;
            this.emailSender = emailSender;
        }

        public IActionResult Send()
        {
            var userEmails = this.contactFormRepository
                .All()
                .Select(x => x.Email)
                .ToList();

            var model = new SendContactInputModel
            {
                UserEmails = userEmails,
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Send(SendContactInputModel sendContactInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                var userEmails = this.contactFormRepository
                    .All()
                    .Select(x => x.Email)
                    .ToList();

                sendContactInputModel.UserEmails = userEmails;

                return this.View(sendContactInputModel);
            }

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

            return this.RedirectToAction("SuccessfullySend", new { userEmail = sendContactInputModel.Email });
        }

        public IActionResult SuccessfullySend(string userEmail)
        {
            return this.View("SuccessfullySend", userEmail);
        }
    }
}
