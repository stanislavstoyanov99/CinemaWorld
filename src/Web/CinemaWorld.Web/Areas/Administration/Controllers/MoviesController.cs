namespace CinemaWorld.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using CinemaWorld.Models.InputModels.AdministratorInputModels.Movies;
    using CinemaWorld.Models.ViewModels.Directors;
    using CinemaWorld.Models.ViewModels.Genres;
    using CinemaWorld.Models.ViewModels.Movies;
    using CinemaWorld.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class MoviesController : AdministrationController
    {
        private readonly IMoviesService moviesService;
        private readonly IDirectorsService directorsService;
        private readonly IGenresService genresService;

        public MoviesController(IMoviesService moviesService, IDirectorsService directorsService, IGenresService genresService)
        {
            this.moviesService = moviesService;
            this.directorsService = directorsService;
            this.genresService = genresService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public async Task<IActionResult> Create()
        {
            var directors = await this.directorsService
                .GetAllDirectorsAsync<DirectorDetailsViewModel>();
            var genres = await this.genresService
                .GetAllGenresAsync<GenreDetailsViewModel>();
            var model = new MovieCreateInputModel
            {
                Directors = directors,
                Genres = genres,
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MovieCreateInputModel movieCreateInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                var directors = await this.directorsService
                    .GetAllDirectorsAsync<DirectorDetailsViewModel>();
                var genres = await this.genresService
                    .GetAllGenresAsync<GenreDetailsViewModel>();

                movieCreateInputModel.Directors = directors;
                movieCreateInputModel.Genres = genres;

                return this.View(movieCreateInputModel);
            }

            await this.moviesService.CreateAsync(movieCreateInputModel);
            return this.RedirectToAction("GetAll", "Movies", new { area = "Administration" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var directors = await this.directorsService
                .GetAllDirectorsAsync<DirectorDetailsViewModel>();
            var genres = await this.genresService
                .GetAllGenresAsync<GenreDetailsViewModel>();
            var movieToEdit = await this.moviesService
                .GetViewModelByIdAsync<MovieEditViewModel>(id);

            movieToEdit.Directors = directors;
            movieToEdit.Genres = genres;

            return this.View(movieToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MovieEditViewModel movieEditViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                var directors = await this.directorsService
                    .GetAllDirectorsAsync<DirectorDetailsViewModel>();
                var genres = await this.genresService
                    .GetAllGenresAsync<GenreDetailsViewModel>();

                movieEditViewModel.Directors = directors;
                movieEditViewModel.Genres = genres;

                return this.View(movieEditViewModel);
            }

            await this.moviesService.EditAsync(movieEditViewModel);
            return this.RedirectToAction("GetAll", "Movies", new { area = "Administration" });
        }

        public async Task<IActionResult> Remove(int id)
        {
            var movieToDelete = await this.moviesService.GetViewModelByIdAsync<MovieDeleteViewModel>(id);
            return this.View(movieToDelete);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(MovieDeleteViewModel movieDeleteViewModel)
        {
            await this.moviesService.DeleteByIdAsync(movieDeleteViewModel.Id);
            return this.RedirectToAction("GetAll", "Movies", new { area = "Administration" });
        }

        public async Task<IActionResult> GetAll()
        {
            var movies = await this.moviesService.GetAllMoviesAsync<MovieDetailsViewModel>();
            return this.View(movies);
        }
    }
}
