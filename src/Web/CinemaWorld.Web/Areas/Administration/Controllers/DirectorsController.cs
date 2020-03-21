namespace CinemaWorld.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using CinemaWorld.Models.InputModels.AdministratorInputModels.Directors;
    using CinemaWorld.Models.ViewModels.Directors;
    using CinemaWorld.Services.Data.Contracts;
    using Microsoft.AspNetCore.Mvc;

    public class DirectorsController : AdministrationController
    {
        private readonly IDirectorsService directorsService;

        public DirectorsController(IDirectorsService directorsService)
        {
            this.directorsService = directorsService;
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
        public async Task<IActionResult> Create(DirectorCreateInputModel directorCreateInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(directorCreateInputModel);
            }

            await this.directorsService.CreateAsync(directorCreateInputModel);
            return this.RedirectToAction("GetAll", "Directors", new { area = "Administration" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var directorToEdit = await this.directorsService
                .GetViewModelByIdAsync<DirectorEditViewModel>(id);

            return this.View(directorToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DirectorEditViewModel directorEditViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(directorEditViewModel);
            }

            await this.directorsService.EditAsync(directorEditViewModel);
            return this.RedirectToAction("GetAll", "Directors", new { area = "Administration" });
        }

        public async Task<IActionResult> Remove(int id)
        {
            var directorToDelete = await this.directorsService.GetViewModelByIdAsync<DirectorDetailsViewModel>(id);
            return this.View(directorToDelete);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(DirectorDetailsViewModel directorViewModel)
        {
            await this.directorsService.DeleteByIdAsync(directorViewModel.Id);
            return this.RedirectToAction("GetAll", "Directors", new { area = "Administration" });
        }

        public async Task<IActionResult> GetAll()
        {
            var directors = await this.directorsService.GetAllDirectorsAsync<DirectorDetailsViewModel>();
            return this.View(directors);
        }
    }
}
