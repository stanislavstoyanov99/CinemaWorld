namespace CinemaWorld.Data.Models
{
    using System;

    using CinemaWorld.Data.Common.Models;

    public class MovieDirector : IDeletableEntity
    {
        public int MovieId { get; set; }

        public virtual Movie Movie { get; set; }

        public int DirectorId { get; set; }

        public virtual Director Director { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
