namespace CinemaWorld.Data.Models
{
    using System;
    using System.Collections.Generic;

    using CinemaWorld.Data.Common.Models;

    public class Promotion : BaseModel<int>, IDeletableEntity
    {
        public Promotion()
        {
            this.SaleTransactions = new HashSet<SaleTransaction>();
        }

        public string Description { get; set; }

        public decimal Discount { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public virtual ICollection<SaleTransaction> SaleTransactions { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
