namespace CinemaWorld.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Data.Common.Models;
    using CinemaWorld.Data.Models.Enumerations;

    public class Hall : BaseModel<int>, IDeletableEntity
    {
        public Hall()
        {
            this.Seats = new HashSet<Seat>();
            this.Projections = new HashSet<MovieProjection>();
        }

        [Required]
        public HallCategory Category { get; set; }

        public virtual ICollection<Seat> Seats { get; set; }

        public virtual ICollection<MovieProjection> Projections { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
