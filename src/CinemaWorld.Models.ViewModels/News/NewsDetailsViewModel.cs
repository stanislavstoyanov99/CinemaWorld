namespace CinemaWorld.Models.ViewModels.News
{
    using System;
    using System.Collections.Generic;

    using CinemaWorld.Data.Models;
    using CinemaWorld.Models.ViewModels.NewsComments;
    using CinemaWorld.Services.Mapping;

    using Ganss.XSS;

    public class NewsDetailsViewModel : IMapFrom<News>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public DateTime CreatedOn { get; set; }

        public string UserUserName { get; set; }

        public string ImagePath { get; set; }

        public int ViewsCounter { get; set; }

        public IEnumerable<PostNewsCommentViewModel> NewsComments { get; set; }
    }
}
