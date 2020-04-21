namespace CinemaWorld.Models.ViewModels.Movies
{
    using System.Collections.Generic;

    public class MoviesHomePageListingViewModel
    {
        public IEnumerable<TopRatingMovieDetailsViewModel> AllMovies { get; set; }

        public IEnumerable<SliderMovieDetailsViewModel> TopMoviesInSlider { get; set; }

        public IEnumerable<TopRatingMovieDetailsViewModel> TopRatingMovies { get; set; }

        public IEnumerable<RecentlyAddedMovieDetailsViewModel> RecentlyAddedMovies { get; set; }

        public IEnumerable<MostPopularDetailsViewModel> MostPopularMovies { get; set; }
    }
}
