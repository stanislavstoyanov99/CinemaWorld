namespace CinemaWorld.Data.Models
{
    using System;

    using CinemaWorld.Data.Common.Models;

    public class TicketOrder : IDeletableEntity
    {
        public int TicketId { get; set; }

        public virtual Ticket Ticket { get; set; }

        public int SellerId { get; set; }

        public virtual Seller Seller { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
