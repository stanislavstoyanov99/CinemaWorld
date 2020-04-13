namespace CinemaWorld.Models.ViewModels.Schedule
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Models.ViewModels.Cinemas;
    using CinemaWorld.Models.ViewModels.MovieProjections;
    using CinemaWorld.Models.ViewModels.Movies;

    using static CinemaWorld.Models.Common.ModelValidation;

    public class ScheduleDetailsViewModel
    {
        public PaginatedList<MovieProjectionDetailsViewModel> MovieProjections { get; set; }

        public IEnumerable<RecentlyAddedMovieDetailsViewModel> LatestMovies { get; set; }

        public IEnumerable<CinemaDetailsViewModel> Cinemas { get; set; }

        [Required(ErrorMessage = EmptyFieldLengthError)]
        public int CinemaId { get; set; }
    }
}
