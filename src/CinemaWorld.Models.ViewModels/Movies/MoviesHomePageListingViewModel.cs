namespace CinemaWorld.Models.ViewModels.Movies
{
    using System.Collections.Generic;

    public class MoviesHomePageListingViewModel
    {
        public IEnumerable<TopRatingMovieDetailsViewModel> AllMovies { get; set; }

        public IEnumerable<TopMovieDetailsViewModel> TopMoviesInSlider { get; set; }

        public IEnumerable<TopRatingMovieDetailsViewModel> TopRatingMovies { get; set; }
    }
}
