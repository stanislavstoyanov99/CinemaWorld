namespace CinemaWorld.Models.ViewModels.Tickets
{
    using CinemaWorld.Data.Models;
    using CinemaWorld.Data.Models.Enumerations;
    using CinemaWorld.Services.Mapping;

    public class TicketDetailsViewModel : IMapFrom<Ticket>
    {
        public int Row { get; set; }

        public int Seat { get; set; }

        public decimal Price { get; set; }

        public TimeSlot TimeSlot { get; set; }

        public TicketType TicketType { get; set; }

        public int Quantity { get; set; }
    }
}
