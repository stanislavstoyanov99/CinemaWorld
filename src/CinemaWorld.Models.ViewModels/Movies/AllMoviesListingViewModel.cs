namespace CinemaWorld.Models.ViewModels.Movies
{
    using System.Collections.Generic;

    public class AllMoviesListingViewModel
    {
        public IEnumerable<MovieDetailsViewModel> AllMovies { get; set; }

        public MovieDetailsViewModel MovieDetailsViewModel { get; set; }
    }
}
