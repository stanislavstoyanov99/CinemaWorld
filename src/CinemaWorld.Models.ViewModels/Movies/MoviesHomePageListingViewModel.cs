namespace CinemaWorld.Models.ViewModels.Movies
{
    using System.Collections.Generic;

    public class MoviesHomePageListingViewModel
    {
        public IEnumerable<MovieDetailsViewModel> AllMovies { get; set; }

        public IEnumerable<TopMovieDetailsViewModel> TopMovies { get; set; }
    }
}
