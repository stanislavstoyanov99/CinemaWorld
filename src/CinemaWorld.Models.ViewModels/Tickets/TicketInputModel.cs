namespace CinemaWorld.Models.ViewModels.Tickets
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static CinemaWorld.Models.Common.ModelValidation;
    using static CinemaWorld.Models.Common.ModelValidation.Ticket;

    public class TicketInputModel
    {
        public int Row { get; set; }

        public int Seat { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = InvalidSeat)]
        public decimal Price { get; set; }

        public string TimeSlot { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        public string TicketType { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = InvalidSeat)]
        public int Quantity { get; set; }

        public DateTime MovieProjectionTime { get; set; }
    }
}
