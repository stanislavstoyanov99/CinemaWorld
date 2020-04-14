namespace CinemaWorld.Models.ViewModels.Schedule
{
    using System.Collections.Generic;

    using CinemaWorld.Models.ViewModels.Cinemas;
    using CinemaWorld.Models.ViewModels.MovieProjections;
    using CinemaWorld.Models.ViewModels.Movies;

    public class ScheduleDetailsViewModel
    {
        public PaginatedList<MovieProjectionDetailsViewModel> MovieProjections { get; set; }

        public IEnumerable<RecentlyAddedMovieDetailsViewModel> LatestMovies { get; set; }

        public IEnumerable<CinemaDetailsViewModel> Cinemas { get; set; }

        public int CinemaName { get; set; }
    }
}
