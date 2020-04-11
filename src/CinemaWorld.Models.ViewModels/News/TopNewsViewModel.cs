namespace CinemaWorld.Models.ViewModels.News
{
    using System;

    using CinemaWorld.Data.Models;
    using CinemaWorld.Services.Mapping;

    public class TopNewsViewModel : IMapFrom<News>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedOnBefore => (int)Math.Round((DateTime.UtcNow - this.CreatedOn).TotalHours);

        public string UserUserName { get; set; }

        public int ViewsCounter { get; set; }
    }
}
