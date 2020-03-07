namespace CinemaWorld.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Data.Common.Models;

    using static CinemaWorld.Data.Common.DataValidation.News;

    public class News : BaseDeletableModel<int>
    {
        public News()
        {
            this.MovieNews = new HashSet<MovieNews>();
        }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public ICollection<MovieNews> MovieNews { get; set; }
    }
}
