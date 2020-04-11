namespace CinemaWorld.Models.ViewModels.News
{
    using System.Collections.Generic;

    using CinemaWorld.Models.ViewModels.Movies;

    public class NewsSingleDetailsViewModel
    {
        public NewsDetailsViewModel News { get; set; }

        public IEnumerable<RecentlyAddedMovieDetailsViewModel> LatestMovies { get; set; }

        public IEnumerable<TopNewsViewModel> TopNews { get; set; }
    }
}
