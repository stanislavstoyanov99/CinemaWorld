namespace CinemaWorld.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Data.Common.Models;
    using CinemaWorld.Data.Models.Enumerations;

    using static CinemaWorld.Data.Common.DataValidation.Hall;

    public class Hall : BaseDeletableModel<int>
    {
        public Hall()
        {
            this.Seats = new HashSet<Seat>();
            this.Projections = new HashSet<MovieProjection>();
        }

        [Required]
        public HallCategory Category { get; set; }

        [Range(CapacityMinLength, CapacityMaxLength)]
        public int Capacity { get; set; }

        public virtual ICollection<Seat> Seats { get; set; }

        public virtual ICollection<MovieProjection> Projections { get; set; }
    }
}
