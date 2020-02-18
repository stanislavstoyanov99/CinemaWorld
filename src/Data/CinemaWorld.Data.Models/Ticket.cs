namespace CinemaWorld.Data.Models
{
    using System;
    using System.Collections.Generic;

    using CinemaWorld.Data.Common.Models;
    using CinemaWorld.Data.Models.Enumerations;

    public class Ticket : BaseModel<string>, IDeletableEntity
    {
        public Ticket()
        {
            this.Id = Guid.NewGuid().ToString();
            this.SaleTransactions = new HashSet<SaleTransaction>();
        }

        public int Row { get; set; }

        public int Seat { get; set; }

        public decimal Price { get; set; }

        public int SellerId { get; set; }

        public virtual Seller Seller { get; set; }

        public TimeSlot TimeSlot { get; set; }

        public TicketType TicketType { get; set; }

        public int Quantity { get; set; }

        public virtual ICollection<SaleTransaction> SaleTransactions { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
