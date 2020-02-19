namespace CinemaWorld.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Data.Common.Models;

    using static CinemaWorld.Data.Common.DataValidation.Genre;

    public class Genre : BaseModel<int>, IDeletableEntity
    {
        public Genre()
        {
            this.MovieGenres = new HashSet<MovieGenre>();
        }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        public virtual ICollection<MovieGenre> MovieGenres { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
