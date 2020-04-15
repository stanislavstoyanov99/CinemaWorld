namespace CinemaWorld.Models.ViewModels.Movies
{
    using System.Collections.Generic;
    using System.Linq;

    public class MoviesListingViewModel
    {
        public PaginatedList<MovieDetailsViewModel> Movies { get; set; }

        public List<string> Alphabet
        {
            get
            {
                var alphabet = Enumerable.Range(65, 26).Select(i => ((char)i).ToString()).ToList();
                alphabet.Insert(0, "All");
                alphabet.Insert(1, "0 - 9");
                return alphabet;
            }
        }

        public string SelectedLetter { get; set; }
    }
}
