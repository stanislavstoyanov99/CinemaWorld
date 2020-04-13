namespace CinemaWorld.Models.ViewModels.Movies
{
    using CinemaWorld.Data.Models;
    using CinemaWorld.Services.Mapping;

    public class MovieCountryViewModel : IMapFrom<MovieCountry>, IMapFrom<Country>
    {
        public Movie Movie { get; set; }

        public int MovieId { get; set; }

        public Country Country { get; set; }

        public int CountryId { get; set; }
    }
}
