namespace CinemaWorld.Data.Models
{
    using System;

    using CinemaWorld.Data.Common.Models;

    public class MovieGenre : IDeletableEntity
    {
        public int MovieId { get; set; }

        public virtual Movie Movie { get; set; }

        public int GenreId { get; set; }

        public virtual Genre Genre { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
