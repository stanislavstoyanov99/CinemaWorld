namespace CinemaWorld.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Data.Common.Models;

    using static CinemaWorld.Data.Common.DataValidation.Promotion;

    public class Promotion : BaseDeletableModel<int>
    {
        public Promotion()
        {
            this.SaleTransactions = new HashSet<SaleTransaction>();
        }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        public decimal Discount { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public virtual ICollection<SaleTransaction> SaleTransactions { get; set; }
    }
}
