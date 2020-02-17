namespace CinemaWorld.Data.Models
{
    using System;
    using System.Collections.Generic;

    using CinemaWorld.Data.Common.Models;

    public class Genre : BaseModel<int>, IDeletableEntity
    {
        public Genre()
        {
            this.MovieGenres = new HashSet<MovieGenre>();
        }

        public string Name { get; set; }

        public virtual ICollection<MovieGenre> MovieGenres { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
