namespace CinemaWorld.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using CinemaWorld.Data.Common.Repositories;
    using CinemaWorld.Data.Models;
    using CinemaWorld.Data.Models.Enumerations;
    using CinemaWorld.Models.ViewModels.Tickets;
    using CinemaWorld.Services.Data.Common;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Services.Mapping;

    using Microsoft.EntityFrameworkCore;

    public class TicketsService : ITicketsService
    {
        private const double MorningHour = 10.30;
        private const double LuchHour = 14;
        private const double AfternoonHour = 17;
        private const double EveningHour = 24;

        private readonly IDeletableEntityRepository<Ticket> ticketsRepository;
        private readonly IDeletableEntityRepository<Seat> seatsRepository;
        private readonly IDeletableEntityRepository<MovieProjection> movieProjectionsRepository;

        public TicketsService(
            IDeletableEntityRepository<Ticket> ticketsRepository,
            IDeletableEntityRepository<Seat> seatsRepository,
            IDeletableEntityRepository<MovieProjection> movieProjectionsRepository)
        {
            this.ticketsRepository = ticketsRepository;
            this.seatsRepository = seatsRepository;
            this.movieProjectionsRepository = movieProjectionsRepository;
        }

        public async Task<TicketDetailsViewModel> BuyAsync(TicketInputModel ticketInputModel, int movieProjectionId)
        {
            if (!Enum.TryParse(ticketInputModel.TicketType, true, out TicketType ticketType))
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.InvalidTicketType, ticketInputModel.TicketType));
            }

            var movieProjection = await this.movieProjectionsRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == movieProjectionId);

            if (movieProjection == null)
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.MovieProjectionNotFound, movieProjectionId));
            }

            var seat = await this.seatsRepository
                .All()
                .FirstOrDefaultAsync(x => x.RowNumber == ticketInputModel.Row &&
                    x.Number == ticketInputModel.Seat && x.HallId == movieProjection.HallId);

            if (seat == null)
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.SeatNotFound, ticketInputModel.Seat));
            }

            if (seat.IsSold)
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.SeatSoldError, seat.Id));
            }

            var ticket = new Ticket
            {
                Row = ticketInputModel.Row,
                Seat = ticketInputModel.Seat,
                Price = ticketInputModel.Price,
                TicketType = ticketType,
                Quantity = ticketInputModel.Quantity,
            };

            bool doesTicketExist = await this.ticketsRepository
                .All()
                .AnyAsync(x => x.Row == ticket.Row && x.Seat == ticket.Seat);
            if (doesTicketExist)
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.TicketAlreadyExists, ticket.Row, ticket.Seat));
            }

            var movieProjectionHour = ticketInputModel.MovieProjectionTime.TimeOfDay.TotalHours;
            this.SetTimeSlot(ticket, movieProjectionHour);

            seat.IsAvailable = false;
            seat.IsSold = true;

            this.seatsRepository.Update(seat);
            await this.seatsRepository.SaveChangesAsync();

            await this.ticketsRepository.AddAsync(ticket);
            await this.ticketsRepository.SaveChangesAsync();

            var viewModel = await this.GetViewModelByIdAsync<TicketDetailsViewModel>(ticket.Id);

            return viewModel;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var ticket = await this.ticketsRepository.All().FirstOrDefaultAsync(x => x.Id == id);
            if (ticket == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.TicketNotFound, id));
            }

            ticket.IsDeleted = true;
            ticket.DeletedOn = DateTime.UtcNow;
            this.ticketsRepository.Update(ticket);
            await this.ticketsRepository.SaveChangesAsync();
        }

        public async Task<TViewModel> GetViewModelByIdAsync<TViewModel>(int id)
        {
            var ticketViewModel = await this.ticketsRepository
                .All()
                .Where(t => t.Id == id)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            if (ticketViewModel == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.TicketNotFound, id));
            }

            return ticketViewModel;
        }

        private void SetTimeSlot(Ticket ticket, double movieProjectionHour)
        {
            if (movieProjectionHour > MorningHour)
            {
                if (movieProjectionHour > MorningHour && movieProjectionHour <= LuchHour)
                {
                    ticket.TimeSlot = TimeSlot.Lunch;
                }
                else if (movieProjectionHour > LuchHour && movieProjectionHour <= AfternoonHour)
                {
                    ticket.TimeSlot = TimeSlot.Afternoon;
                }
                else if (movieProjectionHour > AfternoonHour && movieProjectionHour <= EveningHour)
                {
                    ticket.TimeSlot = TimeSlot.Еvening;
                }
            }
            else
            {
                ticket.TimeSlot = TimeSlot.Morning;
            }
        }
    }
}
