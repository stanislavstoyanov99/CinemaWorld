namespace CinemaWorld.Web.Controllers
{
    using System.Threading.Tasks;

    using CinemaWorld.Common;
    using CinemaWorld.Data.Common.Repositories;
    using CinemaWorld.Data.Models;
    using CinemaWorld.Models.ViewModels.Contacts;
    using CinemaWorld.Services.Messaging;

    using Microsoft.AspNetCore.Mvc;

    public class ContactsController : Controller
    {
        private readonly IRepository<ContactFormEntry> contactsRepository;
        private readonly IEmailSender emailSender;

        public ContactsController(IRepository<ContactFormEntry> contactsRepository, IEmailSender emailSender)
        {
            this.contactsRepository = contactsRepository;
            this.emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ContactFormEntryViewModel contactFormViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(contactFormViewModel);
            }

            var contactFormEntry = new ContactFormEntry
            {
                FirstName = contactFormViewModel.FirstName,
                LastName = contactFormViewModel.LastName,
                Email = contactFormViewModel.Email,
                Subject = contactFormViewModel.Subject,
                Content = contactFormViewModel.Content,
            };

            await this.contactsRepository.AddAsync(contactFormEntry);
            await this.contactsRepository.SaveChangesAsync();

            await this.emailSender.SendEmailAsync(
                contactFormViewModel.Email,
                string.Concat(contactFormViewModel.FirstName, " ", contactFormViewModel.LastName),
                GlobalConstants.SystemEmail,
                contactFormViewModel.Subject,
                contactFormViewModel.Content);

            return this.RedirectToAction("ThankYou");
        }

        public IActionResult ThankYou()
        {
            return this.View();
        }
    }
}
