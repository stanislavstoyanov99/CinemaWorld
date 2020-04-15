namespace CinemaWorld.Models.ViewModels.Movies
{
    public class MoviesListingViewModel
    {
        public PaginatedList<MovieDetailsViewModel> Movies { get; set; }

        public AlphabeticalPagingViewModel AlphabeticalPagingViewModel { get; set; }
    }
}
