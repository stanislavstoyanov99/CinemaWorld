namespace CinemaWorld.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using CinemaWorld.Models.ViewModels;
    using CinemaWorld.Models.ViewModels.MovieProjections;
    using CinemaWorld.Models.ViewModels.Tickets;
    using CinemaWorld.Services.Data.Contracts;

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

                return this.View(viewModel);
            }

            try
            {
                await this.ticketsService.BuyAsync(movieProjectionViewModel.Ticket, id);
            }
            catch (ArgumentException aex)
            {
                return this.View(
                    "BookingError", new BookingErrorViewModel
                    {
                        RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
                        ErrorMessage = aex.Message,
                    });
            }

            return this.RedirectToAction("Index", "Schedule");
        }
    }
}
