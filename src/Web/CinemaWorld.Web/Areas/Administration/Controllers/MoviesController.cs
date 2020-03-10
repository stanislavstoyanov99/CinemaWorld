namespace CinemaWorld.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using CinemaWorld.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class MoviesController : AdministrationController
    {
        private readonly IMoviesService moviesService;

        public MoviesController(IMoviesService moviesService)
        {
            this.moviesService = moviesService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public async Task<IActionResult> Create()
        {
            return this.View();
        }

        public async Task<IActionResult> Edit()
        {
            return this.View();
        }

        public async Task<IActionResult> Remove()
        {
            return this.View();
        }

        public async Task<IActionResult> GetAll()
        {
            return this.View();
        }
    }
}
