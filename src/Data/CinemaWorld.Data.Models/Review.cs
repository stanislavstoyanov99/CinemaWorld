namespace CinemaWorld.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Data.Common.Models;

    using static CinemaWorld.Data.Common.DataValidation.Review;

    public class Review : BaseDeletableModel<int>
    {
        public Review()
        {
            this.MovieReviews = new HashSet<MovieReview>();
            this.Authors = new HashSet<ReviewAuthor>();
        }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        public DateTime Date { get; set; }

        public virtual ICollection<MovieReview> MovieReviews { get; set; }

        public virtual ICollection<ReviewAuthor> Authors { get; set; }
    }
}
