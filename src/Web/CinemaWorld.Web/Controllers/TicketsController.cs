namespace CinemaWorld.Web.Controllers
{
    using System.Threading.Tasks;

    using CinemaWorld.Models.ViewModels.MovieProjections;
    using CinemaWorld.Models.ViewModels.Tickets;
    using CinemaWorld.Services.Data.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class TicketsController : Controller
    {
        private readonly IMovieProjectionsService movieProjectionsService;
        private readonly ITicketsService ticketsService;

        public TicketsController(
            IMovieProjectionsService movieProjectionsService,
            ITicketsService ticketsService)
        {
            this.movieProjectionsService = movieProjectionsService;
            this.ticketsService = ticketsService;
        }

        [Authorize]
        public async Task<IActionResult> Book(int id)
        {
            var movieProjection = await this.movieProjectionsService
                .GetViewModelByIdAsync<MovieProjectionDetailsViewModel>(id);

            var viewModel = new MovieProjectionViewModel
            {
                MovieProjection = movieProjection,
                Ticket = new TicketInputModel
                {
                    MovieProjectionTime = movieProjection.Date,
                },
            };

            return this.View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Book(TicketInputModel ticketInputModel, int movieProjectionId)
        {
            if (!this.ModelState.IsValid)
            {
                var movieProjection = await this.movieProjectionsService
                    .GetViewModelByIdAsync<MovieProjectionDetailsViewModel>(movieProjectionId);

                var viewModel = new MovieProjectionViewModel
                {
                    MovieProjection = movieProjection,
                    Ticket = ticketInputModel,
                };

                return this.View(viewModel);
            }

            await this.ticketsService.BuyAsync(ticketInputModel);
            return this.RedirectToAction("Index", "Schedule");
        }
    }
}
