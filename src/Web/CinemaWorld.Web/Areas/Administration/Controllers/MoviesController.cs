namespace CinemaWorld.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using CinemaWorld.Models.InputModels.AdministratorInputModels.Movies;
    using CinemaWorld.Models.ViewModels.Movies;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Web.Infrastructure;
    using Microsoft.AspNetCore.Mvc;

    public class MoviesController : AdministrationController
    {
        private readonly IMoviesService moviesService;
        private readonly IDirectorsService directorsService;

        public MoviesController(IMoviesService moviesService, IDirectorsService directorsService)
        {
            this.moviesService = moviesService;
            this.directorsService = directorsService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Create()
        {
            var directors = this.directorsService
                .GetAllDirectorsAsync()
                .GetAwaiter()
                .GetResult();
            var model = new MovieCreateInputModel
            {
                Directors = directors,
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MovieCreateInputModel movieCreateInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                var directors = this.directorsService
                .GetAllDirectorsAsync()
                .GetAwaiter()
                .GetResult();
                movieCreateInputModel.Directors = directors;
                return this.View(movieCreateInputModel);
            }

            await this.moviesService.CreateAsync(movieCreateInputModel);
            return this.RedirectToAction("All", "Movies", new { area = string.Empty });
        }

        public IActionResult Edit(int id)
        {
            var directors = this.directorsService
                .GetAllDirectorsAsync()
                .GetAwaiter()
                .GetResult();

            var movieToEdit = this.moviesService
                .GetViewModelByIdAsync<MovieEditViewModel>(id)
                .GetAwaiter()
                .GetResult();
            movieToEdit.Id = id;
            movieToEdit.Directors = directors;

            return this.View(movieToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MovieEditViewModel movieEditViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                var directors = this.directorsService
                .GetAllDirectorsAsync()
                .GetAwaiter()
                .GetResult();

                movieEditViewModel.Directors = directors;
                return this.View(movieEditViewModel);
            }

            await this.moviesService.EditAsync(movieEditViewModel);
            return this.RedirectToAction("All", "Movies", new { area = string.Empty });
        }

        public async Task<IActionResult> Remove()
        {
            return this.View();
        }

        public async Task<IActionResult> GetAll()
        {
            var movies = await this.moviesService.GetAllMoviesAsync<MovieDetailsViewModel>();
            return this.View(movies);
        }
    }
}
