namespace CinemaWorld.Web.Controllers
{
    using System.Threading.Tasks;

    using CinemaWorld.Models.ViewModels;
    using CinemaWorld.Models.ViewModels.Movies;
    using CinemaWorld.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class GenresController : Controller
    {
        private const int GenrePageSize = 2;

        private readonly IMoviesService moviesService;

        public GenresController(IMoviesService moviesService)
        {
            this.moviesService = moviesService;
        }

        public async Task<IActionResult> ByName(int? pageNumber, string name)
        {
            this.TempData["GenreName"] = name;
            var moviesByGenreName = this.moviesService.GetByGenreNameAsQueryable(name);

            var moviesByGenreNamePaginated = await PaginatedList<MovieDetailsViewModel>
                    .CreateAsync(moviesByGenreName, pageNumber ?? 1, GenrePageSize);

            return this.View(moviesByGenreNamePaginated);
        }
    }
}
