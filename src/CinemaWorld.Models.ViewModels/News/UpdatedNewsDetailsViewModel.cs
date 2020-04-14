namespace CinemaWorld.Models.ViewModels.News
{
    using System;

    using CinemaWorld.Data.Models;
    using CinemaWorld.Services.Mapping;

    public class UpdatedNewsDetailsViewModel : IMapFrom<News>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string CreationDate => this.CreatedOn.ToShortDateString();
    }
}
