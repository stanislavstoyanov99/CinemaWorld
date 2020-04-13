namespace CinemaWorld.Models.ViewModels.Movies
{
    using CinemaWorld.Data.Models;
    using CinemaWorld.Services.Mapping;

    public class MovieGenreViewModel : IMapFrom<MovieGenre>, IMapFrom<Genre>
    {
        public Movie Movie { get; set; }

        public int MovieId { get; set; }

        public Genre Genre { get; set; }

        public int GenreId { get; set; }
    }
}
