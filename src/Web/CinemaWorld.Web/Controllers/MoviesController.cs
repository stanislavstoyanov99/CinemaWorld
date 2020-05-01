namespace CinemaWorld.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using CinemaWorld.Models.ViewModels;
    using CinemaWorld.Models.ViewModels.Movies;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Web.Helpers;

    using Microsoft.AspNetCore.Mvc;

    public class MoviesController : Controller
    {
        private const int PageSize = 10;

        private readonly IMoviesService moviesService;

        public MoviesController(IMoviesService moviesService)
        {
            this.moviesService = moviesService;
        }

        public async Task<IActionResult> All(string searchString, string currentFilter, string selectedLetter, int? pageNumber)
        {
            this.ViewData["Current"] = nameof(this.All);

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            this.ViewData["CurrentSearchFilter"] = searchString;
            var movies = this.moviesService
                .GetAllMoviesByFilterAsQueryeable<MovieDetailsViewModel>(selectedLetter);

            if (!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(m => m.Name.ToLower().Contains(searchString.ToLower()));
            }

            var moviesPaginated = await PaginatedList<MovieDetailsViewModel>.CreateAsync(movies, pageNumber ?? 1, PageSize);

            var alphabeticalPagingViewModel = new AlphabeticalPagingViewModel
            {
                SelectedLetter = selectedLetter,
            };

            var viewModel = new MoviesListingViewModel
            {
                Movies = moviesPaginated,
                AlphabeticalPagingViewModel = alphabeticalPagingViewModel,
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var movie = await this.moviesService.GetViewModelByIdAsync<MovieDetailsViewModel>(id);
            var topRatingMovies = await this.moviesService.GetAllMoviesAsync<TopRatingMovieDetailsViewModel>();

            string videoId = ExtractVideoHelper.ExtractVideoId(movie.TrailerPath);
            movie.TrailerPath = videoId;

            var viewModel = new DetailsListingViewModel
            {
                MovieDetailsViewModel = movie,
                AllMovies = topRatingMovies,
            };

            return this.View(viewModel);
        }
    }
}
