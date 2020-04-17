namespace CinemaWorld.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CinemaWorld.Models.ViewModels.MovieProjections;
    using CinemaWorld.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class TicketsController : Controller
    {
        private readonly IMovieProjectionsService movieProjectionsService;

        public TicketsController(IMovieProjectionsService movieProjectionsService)
        {
            this.movieProjectionsService = movieProjectionsService;
        }

        public async Task<IActionResult> Book(int id)
        {
            var movieProjection = await this.movieProjectionsService
                .GetViewModelByIdAsync<MovieProjectionDetailsViewModel>(id);

            return this.View(movieProjection);
        }
    }
}
