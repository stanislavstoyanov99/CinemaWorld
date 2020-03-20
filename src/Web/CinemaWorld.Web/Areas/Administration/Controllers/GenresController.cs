namespace CinemaWorld.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using CinemaWorld.Models.InputModels.AdministratorInputModels.Genres;
    using CinemaWorld.Models.ViewModels.Genres;
    using CinemaWorld.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class GenresController : AdministrationController
    {
        private readonly IGenresService genresService;

        public GenresController(IGenresService genresService)
        {
            this.genresService = genresService;
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
        public async Task<IActionResult> Create(GenreCreateInputModel genreCreateInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(genreCreateInputModel);
            }

            await this.genresService.CreateAsync(genreCreateInputModel);
            return this.RedirectToAction("GetAll", "Genres", new { area = "Administration" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var genreToEdit = await this.genresService
                .GetViewModelByIdAsync<GenreEditViewModel>(id);

            return this.View(genreToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GenreEditViewModel genreEditViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(genreEditViewModel);
            }

            await this.genresService.EditAsync(genreEditViewModel);
            return this.RedirectToAction("GetAll", "Genres", new { area = "Administration" });
        }

        public async Task<IActionResult> Remove(int id)
        {
            var genreToDelete = await this.genresService.GetViewModelByIdAsync<GenreDetailsViewModel>(id);
            return this.View(genreToDelete);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(GenreDetailsViewModel genreDetailsViewModel)
        {
            await this.genresService.DeleteByIdAsync(genreDetailsViewModel.Id);
            return this.RedirectToAction("GetAll", "Genres", new { area = "Administration" });
        }

        public async Task<IActionResult> GetAll()
        {
            var genres = await this.genresService.GetAllGenresAsync<GenreDetailsViewModel>();
            return this.View(genres);
        }
    }
}
