namespace CinemaWorld.Data.Models
{
    using System;
    using System.Collections.Generic;

    using CinemaWorld.Data.Common.Models;

    public class Movie : BaseModel<int>, IDeletableEntity
    {
        public Movie()
        {
            this.MovieGenres = new HashSet<MovieGenre>();
            this.MovieCountries = new HashSet<MovieCountry>();
            this.MovieDirectors = new HashSet<MovieDirector>();
            this.MovieActors = new HashSet<MovieActor>();
        }

        public string Name { get; set; }

        public DateTime DateOfRelease { get; set; }

        public string Status { get; set; }

        public decimal Rating { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public string TrailerPath { get; set; }

        public string CoverPath { get; set; }

        public string IMDBLink { get; set; }

        public int Length { get; set; }

        public int ReviewId { get; set; }

        public virtual Review Review { get; set; }

        public virtual ICollection<MovieGenre> MovieGenres { get; set; }

        public virtual ICollection<MovieCountry> MovieCountries { get; set; }

        public virtual ICollection<MovieDirector> MovieDirectors { get; set; }

        public virtual ICollection<MovieActor> MovieActors { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
