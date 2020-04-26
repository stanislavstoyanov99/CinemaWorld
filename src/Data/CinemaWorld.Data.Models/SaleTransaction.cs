namespace CinemaWorld.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Data.Common.Models;
    using CinemaWorld.Data.Models.Enumerations;

    public class SaleTransaction : BaseDeletableModel<string>
    {
        public SaleTransaction()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public DateTime Date { get; set; }

        public string UserId { get; set; }

        public virtual CinemaWorldUser User { get; set; }

        public int? PromotionId { get; set; }

        public int MovieProjectionId { get; set; }

        public virtual MovieProjection MovieProjection { get; set; }

        public int TicketId { get; set; }

        public virtual Ticket Ticket { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
    }
}
