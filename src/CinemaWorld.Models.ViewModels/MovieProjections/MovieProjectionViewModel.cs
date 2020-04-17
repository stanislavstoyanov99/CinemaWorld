namespace CinemaWorld.Models.ViewModels.MovieProjections
{
    using CinemaWorld.Models.ViewModels.Tickets;

    public class MovieProjectionViewModel
    {
        public MovieProjectionDetailsViewModel MovieProjection { get; set; }

        public TicketInputModel Ticket { get; set; }
    }
}
