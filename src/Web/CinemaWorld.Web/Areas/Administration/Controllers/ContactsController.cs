namespace CinemaWorld.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using CinemaWorld.Models.InputModels.AdministratorInputModels.Contacts;
    using CinemaWorld.Models.ViewModels.Contacts;
    using CinemaWorld.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class ContactsController : AdministrationController
    {
        private readonly IContactsService contactsService;

        public ContactsController(IContactsService contactsService)
        {
            this.contactsService = contactsService;
        }

        public async Task<IActionResult> Send()
        {
            var userEmails = await this.contactsService.GetAllUserEmailsAsync<UserEmailViewModel>();

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
                var userEmails = await this.contactsService.GetAllUserEmailsAsync<UserEmailViewModel>();
                sendContactInputModel.UserEmails = userEmails;

                return this.View(sendContactInputModel);
            }

            await this.contactsService.SendContactToUser(sendContactInputModel);
            return this.RedirectToAction("SuccessfullySend", new { userEmail = sendContactInputModel.Email });
        }

        public IActionResult SuccessfullySend(string userEmail)
        {
            return this.View("SuccessfullySend", userEmail);
        }
    }
}
