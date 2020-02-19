namespace CinemaWorld.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Data.Common.Models;

    using static CinemaWorld.Data.Common.DataValidation.Review;

    public class Review : BaseModel<int>, IDeletableEntity
    {
        public Review()
        {
            this.Movies = new HashSet<Movie>();
            this.Authors = new HashSet<ReviewAuthor>();
        }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        public DateTime Date { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }

        public virtual ICollection<ReviewAuthor> Authors { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
