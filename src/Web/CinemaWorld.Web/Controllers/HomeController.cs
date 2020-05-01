namespace CinemaWorld.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using CinemaWorld.Models.ViewModels;
    using CinemaWorld.Models.ViewModels.Movies;
    using CinemaWorld.Models.ViewModels.Privacy;
    using CinemaWorld.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private const int TopMoviesInHeaderSliderRating = 6;
        private const int TopMoviesRating = 40;

        private const int TopMoviesCount = 12;
        private const int TopMoviesInHeaderSliderCount = 6;
        private const int RecentlyAddedMoviesCount = 12;
        private const int MostPopularMoviesCount = 3;

        private readonly IMoviesService moviesService;
        private readonly IPrivacyService privacyService;

        public HomeController(IMoviesService moviesService, IPrivacyService privacyService)
        {
            this.moviesService = moviesService;
            this.privacyService = privacyService;
        }

        public async Task<IActionResult> Index(string email)
        {
            if (email != null)
            {
                return this.RedirectToAction("ThankYouSubscription", new { email = email });
            }

            var allMovies = await this.moviesService
                .GetAllMoviesAsync<TopRatingMovieDetailsViewModel>();
            var topMoviesInSlider = await this.moviesService
                .GetTopImdbMoviesAsync<SliderMovieDetailsViewModel>(TopMoviesInHeaderSliderRating, TopMoviesInHeaderSliderCount);
            var topRatingMovies = await this.moviesService
                .GetTopRatingMoviesAsync<TopRatingMovieDetailsViewModel>(TopMoviesRating, TopMoviesCount);
            var recentlyAddedMovies = await this.moviesService
                .GetRecentlyAddedMoviesAsync<RecentlyAddedMovieDetailsViewModel>(RecentlyAddedMoviesCount);
            var mostPopularMovies = await this.moviesService
                .GetMostPopularMoviesAsync<MostPopularDetailsViewModel>(MostPopularMoviesCount);

            var viewModel = new MoviesHomePageListingViewModel
            {
                AllMovies = allMovies,
                TopMoviesInSlider = topMoviesInSlider,
                TopRatingMovies = topRatingMovies,
                RecentlyAddedMovies = recentlyAddedMovies,
                MostPopularMovies = mostPopularMovies,
            };

            return this.View(viewModel);
        }

        public IActionResult ThankYouSubscription(string email)
        {
            return this.View("SuccessfullySubscribed", email);
        }

        public async Task<IActionResult> Privacy()
        {
            var privacy = await this.privacyService.GetViewModelAsync<PrivacyDetailsViewModel>();

            return this.View(privacy);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(HttpErrorViewModel errorViewModel)
        {
            if (errorViewModel.StatusCode == 404)
            {
                return this.View(
                "Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
            }

            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }

        public IActionResult HttpError(HttpErrorViewModel errorViewModel)
        {
            if (errorViewModel.StatusCode == 404)
            {
                return this.View(errorViewModel);
            }

            return this.View(
                "Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
