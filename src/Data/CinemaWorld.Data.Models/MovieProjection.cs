namespace CinemaWorld.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Data.Common.Models;

    public class MovieProjection : BaseModel<int>, IDeletableEntity
    {
        public MovieProjection()
        {
            this.SaleTransactions = new HashSet<SaleTransaction>();
        }

        [Required]
        public DateTime Date { get; set; }

        public int MovieId { get; set; }

        public virtual Movie Movie { get; set; }

        public int HallId { get; set; }

        public virtual Hall Hall { get; set; }

        public int CinemaId { get; set; }

        public virtual Cinema Cinema { get; set; }

        public virtual ICollection<SaleTransaction> SaleTransactions { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
