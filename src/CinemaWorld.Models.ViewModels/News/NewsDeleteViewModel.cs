namespace CinemaWorld.Models.ViewModels.News
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Data.Models;
    using CinemaWorld.Services.Mapping;

    using static CinemaWorld.Models.Common.ModelValidation.News;

    public class NewsDeleteViewModel : IMapFrom<News>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime CreatedOn { get; set; }

        [Display(Name = CreationDateDisplayName)]
        public string CreationDate => this.CreatedOn.ToShortDateString();

        [Display(Name = ImagePathDisplayName)]
        public string ImagePath { get; set; }
    }
}
