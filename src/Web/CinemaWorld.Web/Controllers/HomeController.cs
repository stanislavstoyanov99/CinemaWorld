namespace CinemaWorld.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using CinemaWorld.Models.ViewModels;
    using CinemaWorld.Models.ViewModels.Movies;
    using CinemaWorld.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly IMoviesService moviesService;

        public HomeController(IMoviesService moviesService)
        {
            this.moviesService = moviesService;
        }

        public async Task<IActionResult> Index(string email)
        {
            if (email != null)
            {
                return this.RedirectToAction("ThankYouSubscription", new { email = email });
            }

            var viewModel = new MoviesHomePageListingViewModel
            {
                AllMovies = await this.moviesService.GetAllMoviesAsync<MovieDetailsViewModel>(),
                TopMovies = await this.moviesService.GetTopMoviesAsync<TopMovieDetailsViewModel>(),
            };

            return this.View(viewModel);
        }

        public IActionResult ThankYouSubscription(string email)
        {
            return this.View("SuccessfullySubscribed", email);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
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
