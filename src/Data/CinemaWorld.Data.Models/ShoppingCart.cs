namespace CinemaWorld.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using CinemaWorld.Data.Common.Models;

    public class ShoppingCart : BaseModel<int>
    {
        public ShoppingCart()
        {
            this.SaleTransactions = new HashSet<SaleTransaction>();
        }

        public string UserId { get; set; }

        public virtual CinemaWorldUser User { get; set; }

        public virtual ICollection<SaleTransaction> SaleTransactions { get; set; }
    }
}
