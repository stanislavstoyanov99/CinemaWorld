namespace CinemaWorld.Models.ViewModels.Movies
{
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Data.Models;
    using CinemaWorld.Services.Mapping;

    using static CinemaWorld.Models.Common.ModelValidation.Movie;

    public class MovieGenreViewModel : IMapFrom<MovieGenre>, IMapFrom<Genre>
    {
        public Movie Movie { get; set; }

        public int MovieId { get; set; }

        public Genre Genre { get; set; }

        public int GenreId { get; set; }

        [Display(Name = MovieGenreDisplayName)]
        public string GenreName { get; set; }
    }
}
