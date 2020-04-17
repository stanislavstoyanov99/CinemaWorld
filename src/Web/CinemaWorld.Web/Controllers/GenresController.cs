namespace CinemaWorld.Web.Controllers
{
    using System.Threading.Tasks;

    using CinemaWorld.Models.ViewModels;
    using CinemaWorld.Models.ViewModels.Movies;
    using CinemaWorld.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class GenresController : Controller
    {
        private const int GenresPerPage = 12;

        private readonly IMoviesService moviesService;

        public GenresController(IMoviesService moviesService)
        {
            this.moviesService = moviesService;
        }

        public async Task<IActionResult> ByName(int? pageNumber, string name)
        {
            this.TempData["GenreName"] = name;
            var moviesByGenreName = await Task.Run(() => this.moviesService.GetByGenreNameAsQueryable(name));

            var moviesByGenreNamePaginated = await PaginatedList<MovieDetailsViewModel>
                    .CreateAsync(moviesByGenreName, pageNumber ?? 1, GenresPerPage);

            return this.View(moviesByGenreNamePaginated);
        }
    }
}
