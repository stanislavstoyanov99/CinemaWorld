﻿namespace CinemaWorld.Data.Models
{
    using System;
    using System.Collections.Generic;

    using CinemaWorld.Data.Common.Models;

    public class Review : BaseModel<int>, IDeletableEntity
    {
        public Review()
        {
            this.Movies = new HashSet<Movie>();
            this.Authors = new HashSet<ReviewAuthor>();
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }

        public virtual ICollection<ReviewAuthor> Authors { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
