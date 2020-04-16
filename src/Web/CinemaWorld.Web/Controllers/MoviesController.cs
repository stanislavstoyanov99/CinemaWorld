namespace CinemaWorld.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;

    using CinemaWorld.Models.ViewModels;
    using CinemaWorld.Models.ViewModels.Movies;
    using CinemaWorld.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class MoviesController : Controller
    {
        private const int PageSize = 10;
        private const string AllPaginationFilter = "All";
        private const string DigitPaginationFilter = "0 - 9";

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
            var movies = await this.GetMoviesByLetterOrDigit(selectedLetter);

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

            string videoId = this.ExtractVideoId(movie.TrailerPath);
            movie.TrailerPath = videoId;

            var viewModel = new DetailsListingViewModel
            {
                MovieDetailsViewModel = movie,
                AllMovies = topRatingMovies
                    .OrderByDescending(x => x.StarRatingsSum)
                    .ThenByDescending(x => x.DateOfRelease.Year)
                    .ToList(),
            };

            return this.View(viewModel);
        }

        private async Task<IQueryable<MovieDetailsViewModel>> GetMoviesByLetterOrDigit(string selectedLetter)
        {
            var movies = Enumerable.Empty<MovieDetailsViewModel>().AsQueryable();

            if ((!string.IsNullOrEmpty(selectedLetter) && selectedLetter != AllPaginationFilter)
                || selectedLetter == DigitPaginationFilter)
            {
                movies = await Task.Run(
                    () => this.moviesService
                        .GetAllMoviesByLetterOrDigitAsQueryeable<MovieDetailsViewModel>(selectedLetter));
            }
            else
            {
                movies = await Task.Run(
                    () => this.moviesService
                        .GetAllMoviesAsQueryeable<MovieDetailsViewModel>());
            }

            return movies;
        }

        private string ExtractVideoId(string trailerPath)
        {
            var uri = new Uri(trailerPath);
            var query = HttpUtility.ParseQueryString(uri.Query);

            var videoId = string.Empty;

            if (query.AllKeys.Contains("v"))
            {
                videoId = query["v"];
            }
            else
            {
                videoId = uri.Segments.Last();
            }

            return videoId;
        }
    }
}
