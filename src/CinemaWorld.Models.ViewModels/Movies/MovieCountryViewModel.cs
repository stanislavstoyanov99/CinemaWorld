namespace CinemaWorld.Models.ViewModels.Movies
{
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Data.Models;
    using CinemaWorld.Services.Mapping;

    using static CinemaWorld.Models.Common.ModelValidation.Movie;

    public class MovieCountryViewModel : IMapFrom<MovieCountry>, IMapFrom<Country>
    {
        public Movie Movie { get; set; }

        public int MovieId { get; set; }

        public Country Country { get; set; }

        public int CountryId { get; set; }

        [Display(Name = MovieCountryDisplayName)]
        public string CountryName { get; set; }
    }
}
