namespace CinemaWorld.Web.Controllers
{
    using System.Threading.Tasks;

    using CinemaWorld.Models.ViewModels.Contacts;
    using CinemaWorld.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class ContactsController : Controller
    {
        private readonly IContactsService contactsService;

        public ContactsController(IContactsService contactsService)
        {
            this.contactsService = contactsService;
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

            await this.contactsService.SendContactToAdmin(contactFormViewModel);

            return this.RedirectToAction("ThankYou");
        }

        public IActionResult ThankYou()
        {
            return this.View();
        }
    }
}
