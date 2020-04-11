namespace CinemaWorld.Models.ViewModels.News
{
    using System;

    using CinemaWorld.Data.Models;
    using CinemaWorld.Services.Mapping;

    using Ganss.XSS;

    public class AllNewsListingViewModel : IMapFrom<News>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ShortDescription { get; set; }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int CreatedOnBefore => (int)Math.Round((DateTime.UtcNow - this.CreatedOn).TotalHours);

        public string CreationDate => this.CreatedOn.ToShortDateString();

        public string UserUserName { get; set; }

        public string ImagePath { get; set; }

        public int ViewsCounter { get; set; }
    }
}
