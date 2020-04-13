namespace CinemaWorld.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using CinemaWorld.Models.InputModels.AdministratorInputModels.MovieProjections;
    using CinemaWorld.Models.ViewModels;
    using CinemaWorld.Models.ViewModels.Cinemas;
    using CinemaWorld.Models.ViewModels.Halls;
    using CinemaWorld.Models.ViewModels.MovieProjections;
    using CinemaWorld.Models.ViewModels.Movies;
    using CinemaWorld.Services.Data.Contracts;
    using Microsoft.AspNetCore.Mvc;

    public class MovieProjectionsController : AdministrationController
    {
        private const int PageSize = 10;

        private readonly IMovieProjectionsService movieProjectionsService;
        private readonly IMoviesService moviesService;
        private readonly IHallsService hallsService;
        private readonly ICinemasService cinemasService;

        public MovieProjectionsController(
            IMovieProjectionsService movieProjectionsService,
            IMoviesService moviesService,
            IHallsService hallsService,
            ICinemasService cinemasService)
        {
            this.movieProjectionsService = movieProjectionsService;
            this.moviesService = moviesService;
            this.hallsService = hallsService;
            this.cinemasService = cinemasService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public async Task<IActionResult> Create()
        {
            var movies = await this.moviesService
                .GetAllMoviesAsync<MovieDetailsViewModel>();
            var halls = await this.hallsService
               .GetAllHallsAsync<HallDetailsViewModel>();
            var cinemas = await this.cinemasService
               .GetAllCinemasAsync<CinemaDetailsViewModel>();

            var viewModel = new MovieProjectionCreateInputModel
            {
                Movies = movies,
                Halls = halls,
                Cinemas = cinemas,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MovieProjectionCreateInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                var movies = await this.moviesService
                .GetAllMoviesAsync<MovieDetailsViewModel>();
                var halls = await this.hallsService
                   .GetAllHallsAsync<HallDetailsViewModel>();
                var cinemas = await this.cinemasService
                   .GetAllCinemasAsync<CinemaDetailsViewModel>();

                model.Movies = movies;
                model.Halls = halls;
                model.Cinemas = cinemas;

                return this.View(model);
            }

            await this.movieProjectionsService.CreateAsync(model);
            return this.RedirectToAction("GetAll", "MovieProjections", new { area = "Administration" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var movies = await this.moviesService
               .GetAllMoviesAsync<MovieDetailsViewModel>();
            var halls = await this.hallsService
               .GetAllHallsAsync<HallDetailsViewModel>();
            var cinemas = await this.cinemasService
               .GetAllCinemasAsync<CinemaDetailsViewModel>();

            var movieProjectionToEdit = await this.movieProjectionsService
                .GetViewModelByIdAsync<MovieProjectionEditViewModel>(id);

            movieProjectionToEdit.Movies = movies;
            movieProjectionToEdit.Halls = halls;
            movieProjectionToEdit.Cinemas = cinemas;

            return this.View(movieProjectionToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MovieProjectionEditViewModel movieProjectionEditViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                var movies = await this.moviesService
               .GetAllMoviesAsync<MovieDetailsViewModel>();
                var halls = await this.hallsService
                   .GetAllHallsAsync<HallDetailsViewModel>();
                var cinemas = await this.cinemasService
                   .GetAllCinemasAsync<CinemaDetailsViewModel>();

                movieProjectionEditViewModel.Movies = movies;
                movieProjectionEditViewModel.Halls = halls;
                movieProjectionEditViewModel.Cinemas = cinemas;

                return this.View(movieProjectionEditViewModel);
            }

            await this.movieProjectionsService.EditAsync(movieProjectionEditViewModel);
            return this.RedirectToAction("GetAll", "MovieProjections", new { area = "Administration" });
        }

        public async Task<IActionResult> Remove(int id)
        {
            var movieProjectionToDelete = await this.movieProjectionsService
                .GetViewModelByIdAsync<MovieProjectionDetailsViewModel>(id);
            return this.View(movieProjectionToDelete);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(MovieProjectionDetailsViewModel movieProjectionDetailsViewModel)
        {
            await this.movieProjectionsService.DeleteByIdAsync(movieProjectionDetailsViewModel.Id);
            return this.RedirectToAction("GetAll", "MovieProjections", new { area = "Administration" });
        }

        public async Task<IActionResult> GetAll(int? pageNumber)
        {
            var movieProjections = await Task.Run(() => this.movieProjectionsService
                .GetAllMovieProjectionsAsQueryeable<MovieProjectionDetailsViewModel>());

            return this.View(await PaginatedList<MovieProjectionDetailsViewModel>
                .CreateAsync(movieProjections, pageNumber ?? 1, PageSize));
        }
    }
}
