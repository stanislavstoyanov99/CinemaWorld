namespace CinemaWorld.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;

    using CinemaWorld.Models.ViewModels.Movies;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Web.Infrastructure;

    using Microsoft.AspNetCore.Mvc;

    public class MoviesController : Controller
    {
        private const int PageSize = 10;
        private readonly IMoviesService moviesService;

        public MoviesController(IMoviesService moviesService)
        {
            this.moviesService = moviesService;
        }

        public async Task<IActionResult> All(string searchString, int? pageNumber)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }

            this.ViewData["CurrentFilter"] = searchString;
            var movies = this.moviesService.GetAllMoviesAsQueryeable<MovieDetailsViewModel>();

            if (!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(m => m.Name.ToLower().Contains(searchString.ToLower()));
            }

            return this.View(await PaginatedList<MovieDetailsViewModel>.CreateAsync(movies, pageNumber ?? 1, PageSize));
        }

        public async Task<IActionResult> Details(int id)
        {
            var movie = await this.moviesService.GetViewModelByIdAsync<MovieDetailsViewModel>(id);
            var allMovies = await this.moviesService.GetAllMoviesAsync<TopRatingMovieDetailsViewModel>();

            string videoId = ExtractVideoId(movie.TrailerPath);
            movie.TrailerPath = videoId;

            var viewModel = new AllMoviesListingViewModel
            {
                MovieDetailsViewModel = movie,
                AllMovies = allMovies
                    .OrderByDescending(x => x.StarRatingsSum)
                    .ThenByDescending(x => x.DateOfRelease.Year)
                    .ToList(),
            };

            return this.View(viewModel);
        }

        private static string ExtractVideoId(string trailerPath)
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
