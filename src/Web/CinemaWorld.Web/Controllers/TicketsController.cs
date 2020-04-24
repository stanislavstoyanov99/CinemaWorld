namespace CinemaWorld.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using CinemaWorld.Models.ViewModels.MovieProjections;
    using CinemaWorld.Services.Data.Common;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Web.Helpers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class TicketsController : Controller
    {
        private readonly IMovieProjectionsService movieProjectionsService;
        private readonly ITicketsService ticketsService;
        private readonly ISeatsService seatsService;

        public TicketsController(
            IMovieProjectionsService movieProjectionsService,
            ITicketsService ticketsService,
            ISeatsService seatsService)
        {
            this.movieProjectionsService = movieProjectionsService;
            this.ticketsService = ticketsService;
            this.seatsService = seatsService;
        }

        [Authorize]
        public async Task<IActionResult> Book(int id)
        {
            var movieProjection = await this.movieProjectionsService
                .GetViewModelByIdAsync<MovieProjectionDetailsViewModel>(id);

            var soldSeats = await this.seatsService.GetAllSoldSeatsAsync(movieProjection.Hall.Id);
            var availableSeats = await this.seatsService.GetAllAvailableSeatsAsync(movieProjection.Hall.Id);

            var viewModel = new MovieProjectionViewModel
            {
                MovieProjection = movieProjection,
                SoldSeats = soldSeats,
                AvailableSeats = availableSeats,
            };

            return this.View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Book(MovieProjectionViewModel movieProjectionViewModel, [FromRoute]int id)
        {
            if (!this.ModelState.IsValid)
            {
                var movieProjection = await this.movieProjectionsService
                    .GetViewModelByIdAsync<MovieProjectionDetailsViewModel>(id);

                var soldSeats = await this.seatsService.GetAllSoldSeatsAsync(movieProjection.Hall.Id);
                var availableSeats = await this.seatsService.GetAllAvailableSeatsAsync(movieProjection.Hall.Id);

                var viewModel = new MovieProjectionViewModel
                {
                    MovieProjection = movieProjection,
                    SoldSeats = soldSeats,
                    AvailableSeats = availableSeats,
                    Ticket = movieProjectionViewModel.Ticket,
                };

                return this.Json(new
                    {
                        Success = false,
                        Model = viewModel,
                        Message = ModelErrorsHelper.GetModelErrors(this.ModelState),
                    });
            }

            try
            {
                var result = await this.ticketsService.BuyAsync(movieProjectionViewModel.Ticket, id);

                return this.Json(new
                    {
                        Success = true,
                        Message = OperationalMessages.SuccessfullyBookedMovieProjection,
                    });
            }
            catch (ArgumentException aex)
            {
                return this.Json(new { Success = false, Message = aex.Message });
            }
        }
    }
}
