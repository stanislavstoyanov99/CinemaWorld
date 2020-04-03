namespace CinemaWorld.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using CinemaWorld.Models.InputModels.AdministratorInputModels.About;
    using CinemaWorld.Models.ViewModels.About;
    using CinemaWorld.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class AboutController : AdministrationController
    {
        private readonly IAboutService aboutService;

        public AboutController(IAboutService aboutService)
        {
            this.aboutService = aboutService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FaqCreateInputModel faqCreateInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(faqCreateInputModel);
            }

            await this.aboutService.CreateAsync(faqCreateInputModel);
            return this.RedirectToAction("GetAll", "About", new { area = "Administration" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var faqToEdit = await this.aboutService
                .GetViewModelByIdAsync<FaqEditViewModel>(id);

            return this.View(faqToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FaqEditViewModel faqEditViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(faqEditViewModel);
            }

            await this.aboutService.EditAsync(faqEditViewModel);
            return this.RedirectToAction("GetAll", "About", new { area = "Administration" });
        }

        public async Task<IActionResult> Remove(int id)
        {
            var faqToDelete = await this.aboutService.GetViewModelByIdAsync<FaqDetailsViewModel>(id);
            return this.View(faqToDelete);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(FaqDetailsViewModel faqDetailsViewModel)
        {
            await this.aboutService.DeleteByIdAsync(faqDetailsViewModel.Id);
            return this.RedirectToAction("GetAll", "About", new { area = "Administration" });
        }

        public async Task<IActionResult> GetAll()
        {
            var faqs = await this.aboutService.GetAllFaqsAsync<FaqDetailsViewModel>();
            return this.View(faqs);
        }
    }
}
