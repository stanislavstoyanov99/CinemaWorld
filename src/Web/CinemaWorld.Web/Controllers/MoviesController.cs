namespace CinemaWorld.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CinemaWorld.Models.ViewModels.Movies;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Web.Infrastructure;
    using Microsoft.AspNetCore.Mvc;

    public class MoviesController : Controller
    {
        private readonly IMoviesService moviesService;

        public MoviesController(IMoviesService moviesService)
        {
            this.moviesService = moviesService;
        }

        public async Task<IActionResult> All()
        {
            var movies = await this.moviesService.GetAllMoviesAsync<MovieDetailsViewModel>();
            return this.View(movies);
        }
    }
}
