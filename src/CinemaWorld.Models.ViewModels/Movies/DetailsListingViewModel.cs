namespace CinemaWorld.Models.ViewModels.Movies
{
    using System.Collections.Generic;

    public class DetailsListingViewModel
    {
        public IEnumerable<TopRatingMovieDetailsViewModel> AllMovies { get; set; }

        public MovieDetailsViewModel MovieDetailsViewModel { get; set; }
    }
}
