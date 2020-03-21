namespace CinemaWorld.Models.ViewModels.Movies
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Data.Models;
    using CinemaWorld.Data.Models.Enumerations;
    using CinemaWorld.Models.ViewModels.Directors;
    using CinemaWorld.Services.Mapping;

    using static Common.ModelValidation.Movie;

    public class MovieDetailsViewModel : IMapFrom<Movie>
    {
        [Display(Name = IdDisplayName)]
        public int Id { get; set; }

        [Display(Name = NameDisplayName)]
        public string Name { get; set; }

        [Display(Name = DateOfReleaseAllMoviesDisplayName)]
        public DateTime DateOfRelease { get; set; }

        public string Resolution { get; set; }

        public IEnumerable<MovieGenreViewModel> MovieGenres { get; set; }

        public IEnumerable<MovieCountryViewModel> MovieCountries { get; set; }

        [Display(Name = nameof(Director))]
        public DirectorDetailsViewModel Director { get; set; }

        public decimal Rating { get; set; }

        public string Language { get; set; }

        public CinemaCategory CinemaCategory { get; set; }

        public string CoverPath { get; set; }

        public string TrailerPath { get; set; }

        public int Length { get; set; }
    }
}
