namespace CinemaWorld.Data.Models
{
    using System;
    using System.Collections.Generic;

    using CinemaWorld.Data.Common.Models;

    public class News : BaseModel<int>, IDeletableEntity
    {
        public News()
        {
            this.MovieNews = new HashSet<MovieNews>();
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public ICollection<MovieNews> MovieNews { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
