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

        public int? UpdatedSince
        {
            get
            {
                if (this.IsUpdated)
                {
                    var updatedSince = (int)Math.Round((DateTime.UtcNow - this.ModifiedOn).Value.TotalHours);
                    return updatedSince;
                }
                else
                {
                    return null;
                }
            }
        }

        public string CreationDate => this.CreatedOn.ToShortDateString();

        public bool IsUpdated { get; set; }
    }
}
