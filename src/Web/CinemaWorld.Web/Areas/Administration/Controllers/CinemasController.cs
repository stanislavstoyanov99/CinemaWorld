namespace CinemaWorld.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using CinemaWorld.Models.InputModels.AdministratorInputModels.Cinemas;
    using CinemaWorld.Models.ViewModels.Cinemas;
    using CinemaWorld.Services.Data.Contracts;
    using Microsoft.AspNetCore.Mvc;

    public class CinemasController : AdministrationController
    {
        private readonly ICinemasService cinemasService;

        public CinemasController(ICinemasService cinemasService)
        {
            this.cinemasService = cinemasService;
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
        public async Task<IActionResult> Create(CinemaCreateInputModel cinemaCreateInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(cinemaCreateInputModel);
            }

            await this.cinemasService.CreateAsync(cinemaCreateInputModel);
            return this.RedirectToAction("GetAll", "Cinemas", new { area = "Administration" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var cinemaToEdit = await this.cinemasService
                .GetViewModelByIdAsync<CinemaEditViewModel>(id);

            return this.View(cinemaToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CinemaEditViewModel cinemaEditViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(cinemaEditViewModel);
            }

            await this.cinemasService.EditAsync(cinemaEditViewModel);
            return this.RedirectToAction("GetAll", "Cinemas", new { area = "Administration" });
        }

        public async Task<IActionResult> Remove(int id)
        {
            var cinemaToDelete = await this.cinemasService.GetViewModelByIdAsync<CinemaDetailsViewModel>(id);
            return this.View(cinemaToDelete);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(CinemaDetailsViewModel cinemaDetailsViewModel)
        {
            await this.cinemasService.DeleteByIdAsync(cinemaDetailsViewModel.Id);
            return this.RedirectToAction("GetAll", "Cinemas", new { area = "Administration" });
        }

        public async Task<IActionResult> GetAll()
        {
            var cinemas = await this.cinemasService.GetAllCinemasAsync<CinemaDetailsViewModel>();
            return this.View(cinemas);
        }
    }
}
