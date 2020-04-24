namespace CinemaWorld.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Data.Common.Models;
    using CinemaWorld.Data.Models.Enumerations;

    using static CinemaWorld.Data.Common.DataValidation.Movie;

    public class Movie : BaseDeletableModel<int>
    {
        public Movie()
        {
            this.MovieGenres = new HashSet<MovieGenre>();
            this.MovieReviews = new HashSet<MovieReview>();
            this.MovieCountries = new HashSet<MovieCountry>();
            this.MovieActors = new HashSet<MovieActor>();
            this.Projections = new HashSet<MovieProjection>();
            this.MovieComments = new HashSet<MovieComment>();
        }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        public DateTime DateOfRelease { get; set; }

        public string Resolution { get; set; } // HD, SD quality

        public decimal Rating { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        [MaxLength(LanguageMaxLength)]
        public string Language { get; set; }

        [Required]
        public CinemaCategory CinemaCategory { get; set; } // A, B, C, D

        [MaxLength(TrailerPathMaxLength)]
        public string TrailerPath { get; set; }

        [Required]
        [MaxLength(CoverPathMaxLength)]
        public string CoverPath { get; set; }

        [Required]
        [MaxLength(WallpaperPathMaxLength)]
        public string WallpaperPath { get; set; }

        [MaxLength(ImdbLinkMaxLength)]
        public string IMDBLink { get; set; }

        public int Length { get; set; }

        public int DirectorId { get; set; }

        public virtual Director Director { get; set; }

        public virtual ICollection<MovieGenre> MovieGenres { get; set; }

        public virtual ICollection<MovieReview> MovieReviews { get; set; }

        public virtual ICollection<MovieCountry> MovieCountries { get; set; }

        public virtual ICollection<MovieActor> MovieActors { get; set; }

        public virtual ICollection<MovieProjection> Projections { get; set; }

        public virtual ICollection<StarRating> Ratings { get; set; }

        public virtual ICollection<MovieComment> MovieComments { get; set; }
    }
}
