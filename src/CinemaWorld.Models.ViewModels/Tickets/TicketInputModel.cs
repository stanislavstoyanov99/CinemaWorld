namespace CinemaWorld.Models.ViewModels.Tickets
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc;

    using static CinemaWorld.Models.Common.ModelValidation.Ticket;

    public class TicketInputModel
    {
        [HiddenInput]
        public int Row { get; set; }

        [HiddenInput]
        public int Seat { get; set; }

        [HiddenInput]
        public decimal Price { get; set; }

        public string TimeSlot { get; set; }

        [HiddenInput]
        [Required(ErrorMessage = MissingTicketType)]
        public string TicketType { get; set; }

        [HiddenInput]
        [Range(1, int.MaxValue, ErrorMessage = InvalidSeat)]
        public int Quantity { get; set; }

        [HiddenInput]
        public DateTime MovieProjectionTime { get; set; }
    }
}
