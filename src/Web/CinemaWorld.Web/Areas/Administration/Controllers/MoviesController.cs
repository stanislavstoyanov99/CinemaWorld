namespace CinemaWorld.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using CinemaWorld.Models.InputModels.AdministratorInputModels.Movies;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Web.Infrastructure;
    using Microsoft.AspNetCore.Mvc;

    public class MoviesController : AdministrationController
    {
        private readonly IMoviesService moviesService;
        private readonly IDirectorsService directorsService;

        public MoviesController(IMoviesService moviesService, IDirectorsService directorsService)
        {
            this.moviesService = moviesService;
            this.directorsService = directorsService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Create()
        {
            this.ViewData["Directors"] = SelectListGenerator.GetAllDirectors(this.directorsService);

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MovieCreateInputModel movieCreateInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(movieCreateInputModel);
            }

            var movie = await this.moviesService.CreateAsync(movieCreateInputModel);
            return this.RedirectToAction("All", "Movies", new { area = string.Empty });
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
