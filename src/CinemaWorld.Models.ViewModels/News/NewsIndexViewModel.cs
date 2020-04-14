namespace CinemaWorld.Models.ViewModels.News
{
    using System.Collections.Generic;

    public class NewsIndexViewModel
    {
        public PaginatedList<AllNewsListingViewModel> News { get; set; }

        public IEnumerable<UpdatedNewsDetailsViewModel> UpdatedNews { get; set; }

        public IEnumerable<TopNewsViewModel> TopNews { get; set; }
    }
}
