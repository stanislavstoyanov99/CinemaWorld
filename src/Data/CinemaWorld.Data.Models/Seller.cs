namespace CinemaWorld.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Data.Common;
    using CinemaWorld.Data.Common.Models;
    using CinemaWorld.Data.Models.Enumerations;

    public class Seller : BaseDeletableModel<int>
    {
        public Seller()
        {
            this.TicketOrders = new HashSet<TicketOrder>();
        }

        [Required]
        [MaxLength(DataValidation.FullNameMaxLength)]
        public string FullName { get; set; }

        public Gender Gender { get; set; }

        public int Age { get; set; }

        public int? PhoneNumber { get; set; }

        public virtual ICollection<TicketOrder> TicketOrders { get; set; }
    }
}
