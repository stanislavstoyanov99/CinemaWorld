namespace CinemaWorld.Data.Models
{
    using System;
    using System.Collections.Generic;

    using CinemaWorld.Data.Common.Models;

    public class Actor : BaseModel<int>, IDeletableEntity
    {
        public Actor()
        {
            this.MovieActors = new HashSet<MovieActor>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual ICollection<MovieActor> MovieActors { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
