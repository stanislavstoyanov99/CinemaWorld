namespace CinemaWorld.Data.Models
{
    using System;

    using CinemaWorld.Data.Common.Models;

    public class MovieReview : IDeletableEntity
    {
        public int MovieId { get; set; }

        public virtual Movie Movie { get; set; }

        public int? ReviewId { get; set; }

        public virtual Review Review { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
