namespace CinemaWorld.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using CinemaWorld.Models.InputModels.AdministratorInputModels.Movies;
    using CinemaWorld.Models.ViewModels;
    using CinemaWorld.Models.ViewModels.Countries;
    using CinemaWorld.Models.ViewModels.Directors;
    using CinemaWorld.Models.ViewModels.Genres;
    using CinemaWorld.Models.ViewModels.Movies;
    using CinemaWorld.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class MoviesController : AdministrationController
    {
        private const int PageSize = 10;

        private readonly IMoviesService moviesService;
        private readonly IDirectorsService directorsService;
        private readonly IGenresService genresService;
        private readonly ICountriesService countriesService;

        public MoviesController(
            IMoviesService moviesService,
            IDirectorsService directorsService,
            IGenresService genresService,
            ICountriesService countriesService)
        {
            this.moviesService = moviesService;
            this.directorsService = directorsService;
            this.genresService = genresService;
            this.countriesService = countriesService;
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
            var countries = await this.countriesService
                .GetAllCountriesAsync<CountryDetailsViewModel>();
            var model = new MovieCreateInputModel
            {
                Directors = directors,
                Genres = genres,
                Countries = countries,
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
                var countries = await this.countriesService
                    .GetAllCountriesAsync<CountryDetailsViewModel>();

                movieCreateInputModel.Directors = directors;
                movieCreateInputModel.Genres = genres;
                movieCreateInputModel.Countries = countries;

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
            var countries = await this.countriesService
                .GetAllCountriesAsync<CountryDetailsViewModel>();
            var movieGenres = await this.moviesService
                .GetAllMovieGenresAsync<MovieGenreViewModel>();
            var movieCountries = await this.moviesService
                .GetAllMovieCountriesAsync<MovieCountryViewModel>();

            var movieToEdit = await this.moviesService
                .GetViewModelByIdAsync<MovieEditViewModel>(id);

            movieToEdit.Directors = directors;
            movieToEdit.Genres = genres;
            movieToEdit.Countries = countries;
            movieToEdit.MovieGenres = movieGenres;
            movieToEdit.MovieCountries = movieCountries;

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
                var countries = await this.countriesService
                    .GetAllCountriesAsync<CountryDetailsViewModel>();
                var movieGenres = await this.moviesService
                    .GetAllMovieGenresAsync<MovieGenreViewModel>();
                var movieCountries = await this.moviesService
                    .GetAllMovieCountriesAsync<MovieCountryViewModel>();

                movieEditViewModel.Directors = directors;
                movieEditViewModel.Genres = genres;
                movieEditViewModel.Countries = countries;
                movieEditViewModel.MovieGenres = movieGenres;
                movieEditViewModel.MovieCountries = movieCountries;

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

        public async Task<IActionResult> GetAll(int? pageNumber)
        {
            var movies = await Task.Run(() => this.moviesService.GetAllMoviesAsQueryeable<MovieDetailsViewModel>());
            return this.View(await PaginatedList<MovieDetailsViewModel>.CreateAsync(movies, pageNumber ?? 1, PageSize));
        }
    }
}
