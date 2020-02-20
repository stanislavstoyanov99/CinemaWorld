namespace CinemaWorld.Data.Models
{
    using System;

    using CinemaWorld.Data.Common.Models;

    public class ReviewAuthor : IDeletableEntity
    {
        public int AuthorId { get; set; }

        public virtual Author Author { get; set; }

        public int ReviewId { get; set; }

        public virtual Review Review { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
