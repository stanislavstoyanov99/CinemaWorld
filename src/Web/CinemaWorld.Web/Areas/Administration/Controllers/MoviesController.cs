namespace CinemaWorld.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    public class MoviesController : AdministrationController
    {
        public async Task<IActionResult> AddMovieAsync()
        {
            return this.View();
        }

        public async Task<IActionResult> EditMovieAsync()
        {
            return this.View();
        }

        public async Task<IActionResult> RemoveMovieAsync()
        {
            return this.View();
        }

        public async Task<IActionResult> GetAllMoviesAsync()
        {
            return this.View();
        }
    }
}
