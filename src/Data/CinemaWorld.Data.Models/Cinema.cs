namespace CinemaWorld.Data.Models
{
    using System;
    using System.Collections.Generic;

    using CinemaWorld.Data.Common.Models;

    public class Cinema : BaseModel<int>, IDeletableEntity
    {
        public Cinema()
        {
            this.Projections = new HashSet<MovieProjection>();
        }

        public string Name { get; set; }

        public string Address { get; set; }

        public virtual ICollection<MovieProjection> Projections { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
