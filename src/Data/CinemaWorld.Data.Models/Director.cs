namespace CinemaWorld.Data.Models
{
    using System;
    using System.Collections.Generic;

    using CinemaWorld.Data.Common.Models;

    public class Director : BaseModel<int>, IDeletableEntity
    {
        public Director()
        {
            this.MovieDirectors = new HashSet<MovieDirector>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual ICollection<MovieDirector> MovieDirectors { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
