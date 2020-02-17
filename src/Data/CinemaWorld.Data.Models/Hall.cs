namespace CinemaWorld.Data.Models
{
    using System;
    using System.Collections.Generic;

    using CinemaWorld.Data.Common.Models;
    using CinemaWorld.Data.Models.Enumerations;

    public class Hall : BaseModel<int>, IDeletableEntity
    {
        public Hall()
        {
            this.Seats = new HashSet<Seat>();
        }

        public HallCategory Category { get; set; }

        public virtual ICollection<Seat> Seats { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
