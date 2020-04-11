namespace CinemaWorld.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using CinemaWorld.Models.InputModels.AdministratorInputModels.Halls;
    using CinemaWorld.Models.ViewModels.Halls;
    using CinemaWorld.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class HallsController : AdministrationController
    {
        private readonly IHallsService hallsService;

        public HallsController(IHallsService hallsService)
        {
            this.hallsService = hallsService;
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
        public async Task<IActionResult> Create(HallCreateInputModel hallCreateInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(hallCreateInputModel);
            }

            await this.hallsService.CreateAsync(hallCreateInputModel);
            return this.RedirectToAction("GetAll", "Halls", new { area = "Administration" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var hallToEdit = await this.hallsService
                .GetViewModelByIdAsync<HallEditViewModel>(id);

            return this.View(hallToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(HallEditViewModel hallEditViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(hallEditViewModel);
            }

            await this.hallsService.EditAsync(hallEditViewModel);
            return this.RedirectToAction("GetAll", "Halls", new { area = "Administration" });
        }

        public async Task<IActionResult> Remove(int id)
        {
            var hallToDelete = await this.hallsService.GetViewModelByIdAsync<HallDetailsViewModel>(id);
            return this.View(hallToDelete);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(HallDetailsViewModel hallDetailsViewModel)
        {
            await this.hallsService.DeleteByIdAsync(hallDetailsViewModel.Id);
            return this.RedirectToAction("GetAll", "Halls", new { area = "Administration" });
        }

        public async Task<IActionResult> GetAll()
        {
            var halls = await this.hallsService.GetAllHallsAsync<HallDetailsViewModel>();
            return this.View(halls);
        }
    }
}
