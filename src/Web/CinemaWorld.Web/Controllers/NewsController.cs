namespace CinemaWorld.Web.Controllers
{
    using System.Threading.Tasks;

    using CinemaWorld.Models.ViewModels;
    using CinemaWorld.Models.ViewModels.Movies;
    using CinemaWorld.Models.ViewModels.News;
    using CinemaWorld.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class NewsController : Controller
    {
        private const int NewsCount = 6;
        private const int LatestMoviesCount = 6;
        private const int TopMoviesCount = 10;

        private readonly INewsService newsService;
        private readonly IMoviesService moviesService;

        public NewsController(INewsService newsService, IMoviesService moviesService)
        {
            this.newsService = newsService;
            this.moviesService = moviesService;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            var news = await Task.Run(
                () => this.newsService.GetAllNewsAsQueryeable<AllNewsListingViewModel>());

            var newsPaginated = await PaginatedList<AllNewsListingViewModel>
                .CreateAsync(news, pageNumber ?? 1, NewsCount);
            var updatedNews = await this.newsService
                .GetUpdatedNewsAsync<UpdatedNewsDetailsViewModel>();
            var topNews = await this.newsService
                .GetTopNewsAsync<TopNewsViewModel>(TopMoviesCount);

            var viewModel = new NewsIndexViewModel
            {
                News = newsPaginated,
                UpdatedNews = updatedNews,
                TopNews = topNews,
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var news = await this.newsService
                .SetViewsCounter(id);
            var latestMovies = await this.moviesService
                .GetRecentlyAddedMoviesAsync<RecentlyAddedMovieDetailsViewModel>(LatestMoviesCount);
            var topNews = await this.newsService
                .GetTopNewsAsync<TopNewsViewModel>(TopMoviesCount);

            var viewModel = new NewsSingleDetailsViewModel
            {
                News = news,
                LatestMovies = latestMovies,
                TopNews = topNews,
            };

            return this.View(viewModel);
        }
    }
}
