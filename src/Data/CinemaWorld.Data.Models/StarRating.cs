namespace CinemaWorld.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using CinemaWorld.Data.Common.Models;

    public class StarRating : BaseDeletableModel<int>
    {
        public int Rate { get; set; }

        public int MovieId { get; set; }

        public virtual Movie Movie { get; set; }

        [Required]
        public string UserId { get; set; }

        public CinemaWorldUser User { get; set; }

        public DateTime NextVoteDate { get; set; }
    }
}
